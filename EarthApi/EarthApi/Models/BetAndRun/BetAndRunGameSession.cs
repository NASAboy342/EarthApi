using System;
using EarthApi.Enums;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class BetAndRunGameSession
{
    public string SessionId { get; set; }
    public string Username { get; set; }
    public DateTime StartTime { get; set; }
    public int ReachableTile { get; set; }
    public int CurrentTile { get; set; }
    public bool IsGameOver { get; set; }
    public int GameId { get; set; }
    public EnumBetAndRunGameStatus PreviousGameState { get; set; }
    public EnumBetAndRunGameStatus GameState { get; set; }
    public List<decimal> TileValues { get; set; }
    public decimal Stake { get; set; }
    public DateTime SettleTime { get; set; }
    public decimal SettledAmount { get; set; }
    public EnumBetStatus BetStatus { get; set; }
}
