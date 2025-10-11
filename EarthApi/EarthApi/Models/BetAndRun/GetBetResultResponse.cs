using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class GetBetResultResponse : EarthApiResponseBase
{
    public int CurrentTile { get; set; }
    public bool IsGameOver { get; set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; set; }
    public EnumBetAndRunGameStatus GameState { get; set; }
    public decimal CashOutAmount { get; set; }
}
