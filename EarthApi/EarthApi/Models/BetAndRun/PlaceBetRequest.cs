using System;

namespace EarthApi.Models.BetAndRun;

public class PlaceBetRequest
{
    public string Username { get; set; } = "";
    public decimal Amount { get; set; }

    public void ValidateRequest()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new Exception("Username is required.");

        if (Amount <= 0)
            throw new Exception("Amount must be greater than zero.");
    }
}
