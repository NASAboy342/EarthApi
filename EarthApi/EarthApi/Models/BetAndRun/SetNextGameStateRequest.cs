using System;
using EarthApi.Enums.BetAndRun;

namespace EarthApi.Models.BetAndRun;

public class SetNextGameStateRequest
{
    public string Username { get; set; } = "";
    public EnumBetAndRunGameStatus PreviousGameState { get; set; }
    public EnumBetAndRunGameStatus NextGameState { get; set; }
    internal void ValidateRequest()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new Exception("Username is required.");
    }
}
