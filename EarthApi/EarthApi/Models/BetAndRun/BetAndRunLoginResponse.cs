using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class BetAndRunLoginResponse : EarthApiResponseBase
{
    public string? SessionId { get; set; }
    public int CurrentTile { get; set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; set; }
    public EnumBetAndRunGameStatus GameState { get; set; }
    public List<decimal> TileValues { get; set; } = new List<decimal>();
}
