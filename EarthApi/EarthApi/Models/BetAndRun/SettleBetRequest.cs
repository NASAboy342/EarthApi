using System;

namespace EarthApi.Models.BetAndRun;

public class SettleBetRequest
{
    public string Username { get; set; } = "";

    internal void ValidateRequest()
    {
        if (string.IsNullOrEmpty(Username))
            throw new ArgumentException("Username is required.");
    }
}
