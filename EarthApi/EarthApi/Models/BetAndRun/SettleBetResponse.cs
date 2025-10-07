using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class SettleBetResponse: EarthApiResponseBase
{
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "";
    public int CurrentTile { get; internal set; }
    public bool IsGameOver { get; internal set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; internal set; }
    public EnumBetAndRunGameStatus GameState { get; internal set; }
}
