using System;
using EarthApi.Caches;
using EarthApi.Enums;
using EarthApi.Models;
using EarthApi.Models.BetAndRun;
using EarthApi.Servicies;
using Microsoft.AspNetCore.Mvc;

namespace EarthApi.Controllers;

[ApiController]
[Route("bet-and-run")]
public class BetAndRunController : ControllerBase
{
    private readonly IBetAndRunService _betAndRunService;
    private readonly OnlinePlayerCache _onlinePlayerCache;
    private readonly PlayerBalanceCache _playerBalanceCache;

    public BetAndRunController(IBetAndRunService betAndRunService, OnlinePlayerCache onlinePlayerCache, PlayerBalanceCache playerBalanceCache)
    {
        _betAndRunService = betAndRunService;
        _onlinePlayerCache = onlinePlayerCache;
        _playerBalanceCache = playerBalanceCache;
    }

    [HttpPost("login")]
    public EarthApiResponse<BetAndRunLoginResponse> Login([FromBody] BetAndRunLoginRequest request)
    {
        request.ValidateRequest();
        _betAndRunService.DoIfPlayerNotOnline(request);
        _betAndRunService.DoIfPlayerIsStillInOtherGame(request);
        _betAndRunService.StartNewGameSession(request);

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new BetAndRunLoginResponse()
        {
            SessionId = gameSession.SessionId,
            CurrentTile = gameSession.CurrentTile,
            PreviousGameState = gameSession.PreviousGameState,
            GameState = gameSession.GameState,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Login successful"
        };

        return new EarthApiResponse<BetAndRunLoginResponse>(response);
    }

    [HttpPost("get-current-tile")]
    public EarthApiResponse<GetCurrentTileResponse> GetCurrentTile([FromBody] GetCurrentTileRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        var response = new GetCurrentTileResponse
        {
            CurrentTile = gameSession.CurrentTile,
            IsGameOver = gameSession.IsGameOver,
            PreviousGameState = gameSession.PreviousGameState,
            GameState = gameSession.GameState,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Current tile fetched successfully"
        };

        return new EarthApiResponse<GetCurrentTileResponse>(response);
    }

    [HttpPost("move-to-next-tile")]
    public EarthApiResponse<MoveToNextTileResponse> MoveToNextTile([FromBody] MoveToNextTileRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        _betAndRunService.MovePlayerToNextTile(gameSession);

        var updatedGameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new MoveToNextTileResponse
        {
            CurrentTile = updatedGameSession.CurrentTile,
            IsGameOver = updatedGameSession.IsGameOver,
            PreviousGameState = updatedGameSession.PreviousGameState,
            GameState = updatedGameSession.GameState,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Moved to the next tile successfully"
        };

        return new EarthApiResponse<MoveToNextTileResponse>(response);
    }

    [HttpPost("set-next-game-state")]
    public EarthApiResponse<SetNextGameStateResponse> SetNextGameState([FromBody] SetNextGameStateRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        _betAndRunService.SetNextGameState(gameSession, request);

        var updatedGameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new SetNextGameStateResponse
        {
            CurrentTile = updatedGameSession.CurrentTile,
            IsGameOver = updatedGameSession.IsGameOver,
            PreviousGameState = updatedGameSession.PreviousGameState,
            GameState = updatedGameSession.GameState,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Game state updated successfully"
        };

        return new EarthApiResponse<SetNextGameStateResponse>(response);
    }

    [HttpPost("place-bet")]
    public EarthApiResponse<PlaceBetResponse> PlaceBet([FromBody] PlaceBetRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        _betAndRunService.PlaceBet(gameSession, request);

        var playerBalance = _playerBalanceCache.GetByUserName(request.Username);
        if (playerBalance == null)
            throw new Exception("Player balance not found.");

        var updatedGameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new PlaceBetResponse
        {
            CurrentTile = updatedGameSession.CurrentTile,
            IsGameOver = updatedGameSession.IsGameOver,
            PreviousGameState = updatedGameSession.PreviousGameState,
            GameState = updatedGameSession.GameState,
            Balance = playerBalance.Amount,
            Currency = playerBalance.Currency,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Bet placed successfully"
        };

        return new EarthApiResponse<PlaceBetResponse>(response);
    }

    [HttpPost("get-bet-result")]
    public EarthApiResponse<GetBetResultResponse> GetBetResult([FromBody] GetBetResultRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        _betAndRunService.GetBetResult(gameSession, request);

        var playerBalance = _playerBalanceCache.GetByUserName(request.Username);
        if (playerBalance == null)
            throw new Exception("Player balance not found.");

        var updatedGameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new GetBetResultResponse
        {
            CurrentTile = updatedGameSession.CurrentTile,
            IsGameOver = updatedGameSession.IsGameOver,
            PreviousGameState = updatedGameSession.PreviousGameState,
            GameState = updatedGameSession.GameState,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Bet settled successfully"
        };

        return new EarthApiResponse<GetBetResultResponse>(response);
    }

    [HttpPost("settle-bet")]
    public EarthApiResponse<SettleBetResponse> SettleBet([FromBody] SettleBetRequest request)
    {
        request.ValidateRequest();
        if (!_onlinePlayerCache.IsPlayerOnlined(request.Username, out var playerInfo))
            throw new Exception("Player is not online.");

        var gameSession = _betAndRunService.GetCurrentGameSession(request.Username);
        if (gameSession == null)
            throw new Exception("No active game session found for the player.");

        _betAndRunService.SettleBet(gameSession, request);

        var playerBalance = _playerBalanceCache.GetByUserName(request.Username);
        if (playerBalance == null)
            throw new Exception("Player balance not found.");

        var updatedGameSession = _betAndRunService.GetCurrentGameSession(request.Username);

        var response = new SettleBetResponse
        {
            CurrentTile = updatedGameSession.CurrentTile,
            IsGameOver = updatedGameSession.IsGameOver,
            PreviousGameState = updatedGameSession.PreviousGameState,
            GameState = updatedGameSession.GameState,
            Balance = playerBalance.Amount,
            Currency = playerBalance.Currency,
            ErrorCode = EnumEarthApiErrorCode.Success,
            ExtraMessage = "Bet settled successfully"
        };

        return new EarthApiResponse<SettleBetResponse>(response);
    }
}
