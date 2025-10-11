using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class PlaceBetResponse : EarthApiResponseBase
{
    public int CurrentTile { get; internal set; }
    public bool IsGameOver { get; internal set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; internal set; }
    public EnumBetAndRunGameStatus GameState { get; internal set; }

    public decimal Balance { get; set; }
    public string Currency { get; set; } = "";
    public decimal CashOutAmount { get; set; }
}
