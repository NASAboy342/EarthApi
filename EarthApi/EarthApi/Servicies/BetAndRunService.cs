using System;
using System.Collections.Generic;
using EarthApi.Caches;
using EarthApi.Enums;
using EarthApi.Enums.BetAndRun;
using EarthApi.Helpers;
using EarthApi.Models.BetAndRun;
using EarthApi.Models.Player;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using Newtonsoft.Json;

namespace EarthApi.Servicies;

public class BetAndRunService : IBetAndRunService
{
    private readonly OnlinePlayerCache _onlinePlayerCache;
    private readonly IPlayerService _playerService;
    private readonly int _tileCount = 15;
    private readonly int _stakeMultiplyRate = 10;
    private readonly int _winRate = 80;
    private readonly Random _random;
    private readonly int _gameId = 1;
    private readonly ILoggerService _loggerService;

    public BetAndRunService(OnlinePlayerCache onlinePlayerCache, IPlayerService playerService, ILoggerService loggerService)
    {
        _onlinePlayerCache = onlinePlayerCache;
        _playerService = playerService;
        _loggerService = loggerService;
        _random = new Random();
    }

    public void DoIfPlayerIsStillInOtherGame(BetAndRunLoginRequest request)
    {
        var playerInfo = _onlinePlayerCache.GetByUserName(request.Username);
        if (playerInfo != null && !string.IsNullOrEmpty(playerInfo.GameSessionInJson))
        {
            _onlinePlayerCache.ExitGameSession(request.Username);
        }
    }

    public void DoIfPlayerNotOnline(BetAndRunLoginRequest request)
    {
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var player))
        {
            _playerService.LoginPlayer(request.Username);
        }
    }

    public BetAndRunGameSession GetCurrentGameSession(string username)
    {
        var playerInfo = _onlinePlayerCache.GetByUserName(username);
        if (playerInfo != null && !string.IsNullOrEmpty(playerInfo.GameSessionInJson))
        {
            var deserializeGameSession = JsonConvert.DeserializeObject<BetAndRunGameSession>(playerInfo.GameSessionInJson);
            if (deserializeGameSession != null && deserializeGameSession.GameId == _gameId)
            {
                return deserializeGameSession;
            }
        }
        throw new Exception("Player is not in a game session.");
    }

    public void MovePlayerToNextTile(BetAndRunGameSession gameSession)
    {
        if (gameSession.GameState != EnumBetAndRunGameStatus.RaisingBet)
            throw new Exception("Player is not in the correct game state to move to the next tile. Expected state: RaisingBet");

        if (gameSession.CurrentTile >= _tileCount || gameSession.IsGameOver)
            throw new Exception("Game is already over.");

        gameSession.CurrentTile += 1;
        var playerInfo = _onlinePlayerCache.GetByUserName(gameSession.Username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    public void PlaceBet(BetAndRunGameSession gameSession, PlaceBetRequest request)
    {
        if (gameSession.GameState != EnumBetAndRunGameStatus.AwaitingBet)
            throw new Exception("Player is not in the correct game state to place a bet. Expected state: AwaitingBet");

        var deductResult = _playerService.Deduct(new DeductRequest
        {
            Amount = request.Amount,
            Username = gameSession.Username,
        });

        gameSession.Stake = request.Amount;
        gameSession.CashOutAmount = GetCashOutAmount(gameSession);

        var playerInfo = _onlinePlayerCache.GetByUserName(gameSession.Username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    public void SetNextGameState(BetAndRunGameSession gameSession, SetNextGameStateRequest request)
    {
        if (gameSession.PreviousGameState != request.PreviousGameState)
            throw new Exception("Previous game state does not match.");

        gameSession.PreviousGameState = gameSession.GameState;
        gameSession.GameState = request.NextGameState;

        var playerInfo = _onlinePlayerCache.GetByUserName(gameSession.Username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    public void GetBetResult(BetAndRunGameSession gameSession, GetBetResultRequest request)
    {
        if (gameSession.GameState != EnumBetAndRunGameStatus.SettlingBet)
            throw new Exception("Player is not in the correct game state to settle a bet. Expected state: SettlingBet");

        if (gameSession.CurrentTile >= gameSession.ReachableTile)
        {
            SetNextGameState(request.Username, EnumBetAndRunGameStatus.BetSettledLose);
        }
        else
        {
            UpdateCashOutAmount(gameSession);
            SetNextGameState(request.Username, EnumBetAndRunGameStatus.BetSettledWin);
        }
    }

    private void UpdateCashOutAmount(BetAndRunGameSession gameSession)
    {
        var cashOutAmount = GetCashOutAmount(gameSession);
        gameSession.CashOutAmount = cashOutAmount;
        var playerInfo = _onlinePlayerCache.GetByUserName(gameSession.Username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    private void SetNextGameState(string username, EnumBetAndRunGameStatus newGameStatus)
    {
        var gameSession = GetCurrentGameSession(username);
        gameSession.PreviousGameState = gameSession.GameState;
        gameSession.GameState = newGameStatus;

        var playerInfo = _onlinePlayerCache.GetByUserName(username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    public void StartNewGameSession(BetAndRunLoginRequest request)
    {
        var playerInfo = _onlinePlayerCache.GetByUserName(request.Username);
        if (playerInfo == null)
            throw new Exception("Player is not online.");

        var reachableTile = GetReachableTileForPlayer();

        var tileValues = GetTileValue();

        var newGameSession = new BetAndRunGameSession
        {
            GameId = _gameId,
            SessionId = GameSessionHelper.GenerateSessionId("BetAndRun"),
            Username = playerInfo.Username,
            StartTime = DateTime.UtcNow,
            ReachableTile = reachableTile,
            CurrentTile = 0,
            IsGameOver = false,
            PreviousGameState = EnumBetAndRunGameStatus.None,
            GameState = EnumBetAndRunGameStatus.Start,
            TileValues = tileValues
        };

        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(newGameSession);
    }

    private int GetReachableTileForPlayer()
    {
        var isReachLose = false;
        var reachableTile = 0;
        for (int i = 1; i <= _tileCount && !isReachLose; i++)
        {
            var isWin = GetResult();
            if (isWin)
            {
                reachableTile = i;
            }
            else
            {
                isReachLose = true;
            }
        }
        return reachableTile;
    }

    private bool GetResult()
    {
        var isWin = _winRate > _random.Next(0, 101);
        return isWin;
    }

    public void SettleBet(BetAndRunGameSession gameSession, SettleBetRequest request)
    {
        if (gameSession.GameState != EnumBetAndRunGameStatus.GameOver && gameSession.GameState != EnumBetAndRunGameStatus.CashOut)
            throw new Exception("Player is not in the correct game state to settle a bet. Expected state: GameOver or CashOut");

        if (gameSession.CurrentTile <= 0)
            throw new Exception("Current tile must be greater than 0 to settle a bet or player must take at least one step.");

        var isOnReachableTile = gameSession.CurrentTile <= gameSession.ReachableTile;

        var settleAmount = isOnReachableTile ? GetCashOutAmount(gameSession) : 0;

        var betStatus = settleAmount <= gameSession.Stake ? EnumBetStatus.Lose : EnumBetStatus.Win;

        _playerService.Settle(new SettleRequest
        {
            Amount = settleAmount,
            Username = gameSession.Username,
            BetStatus = betStatus,
            Stake = gameSession.Stake
        });

        gameSession.SettleTime = DateTime.UtcNow;
        gameSession.IsGameOver = true;
        gameSession.SettledAmount = settleAmount;
        gameSession.BetStatus = betStatus;

        var playerInfo = _onlinePlayerCache.GetByUserName(gameSession.Username);
        playerInfo.GameSessionInJson = JsonConvert.SerializeObject(gameSession);
        _onlinePlayerCache.Set(playerInfo);
    }

    private decimal GetCashOutAmount(BetAndRunGameSession gameSession)
    {
        var stakeMultiplier = 0m;
        var targetTileIndex = gameSession.CurrentTile - 1;
        if (targetTileIndex >= 0 && targetTileIndex < gameSession.TileValues.Count)
        {
            stakeMultiplier = gameSession.TileValues[targetTileIndex];
        }
        else
        {
            _loggerService.Error($"Invalid tile index: {targetTileIndex}");
            stakeMultiplier = 0m;
        }
        var cashOutAmount = (gameSession.Stake * stakeMultiplier);
        return cashOutAmount;
    }

    private List<decimal> GetTileValue() {
        var tileValues = new List<decimal>
        {
            (decimal)_stakeMultiplyRate / 100m
        };
        for (var i = 1; i < _tileCount; i++)
        {
            tileValues.Add((tileValues[i - 1] * 100 * 2) / 100);
        }
        return tileValues;
    }
}
