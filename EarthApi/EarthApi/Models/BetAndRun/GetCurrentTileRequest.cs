using System;

namespace EarthApi.Models.BetAndRun;

public class GetCurrentTileRequest
{
    public string Username { get; set; } = "";
    internal void ValidateRequest()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new Exception("Username is required.");
    }
}
