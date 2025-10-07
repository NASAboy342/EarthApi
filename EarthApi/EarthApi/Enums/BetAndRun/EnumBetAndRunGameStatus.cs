using System;

namespace EarthApi.Enums.BetAndRun;

public enum EnumBetAndRunGameStatus
{
    None,
    Start,
    AwaitingBet,
    AwaitingRaiseBet,
    SettlingBet,
    BetSettledWin,
    BetSettledLose,
    CashingOut,
    CashOut,
    GameOver,
    ProceedStartNewGame
}
