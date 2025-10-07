using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class GetBetResultResponse : EarthApiResponseBase
{
    public int CurrentTile { get; internal set; }
    public bool IsGameOver { get; internal set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; internal set; }
    public EnumBetAndRunGameStatus GameState { get; internal set; }
}
