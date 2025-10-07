using System;
using EarthApi.Models.BetAndRun;

namespace EarthApi.Servicies;

public interface IBetAndRunService
{
    void DoIfPlayerIsStillInOtherGame(BetAndRunLoginRequest request);
    void DoIfPlayerNotOnline(BetAndRunLoginRequest request);
    BetAndRunGameSession GetCurrentGameSession(string username);
    void MovePlayerToNextTile(BetAndRunGameSession gameSession);
    void PlaceBet(BetAndRunGameSession gameSession, PlaceBetRequest request);
    void SetNextGameState(BetAndRunGameSession gameSession, SetNextGameStateRequest request);
    void GetBetResult(BetAndRunGameSession gameSession, GetBetResultRequest request);
    void StartNewGameSession(BetAndRunLoginRequest request);
    void SettleBet(BetAndRunGameSession gameSession, SettleBetRequest request);
}
