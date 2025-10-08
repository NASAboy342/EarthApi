using System;

namespace EarthApi.Enums.BetAndRun;

public enum EnumBetAndRunGameStatus
{
    None,
    Start,
    AwaitingBet,
    AwaitingRaiseBet,
    RaisingBet,
    SettlingBet,
    BetSettledWin,
    BetSettledLose,
    CashingOut,
    CashOut,
    GameOver,
    ProceedStartNewGame
}
