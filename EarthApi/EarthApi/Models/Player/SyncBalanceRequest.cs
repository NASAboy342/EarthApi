using System;

namespace EarthApi.Models.Player;

public class SyncBalanceRequest
{
    public string Username { get; set; } = "";

    public void ValidateRequest()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new Exception("Username is required.");
    }
}
