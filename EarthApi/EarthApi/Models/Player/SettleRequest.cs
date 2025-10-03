using System;
using EarthApi.Enums;

namespace EarthApi.Models.Player;

public class SettleRequest
{
    public string Username { get; set; } = "";
    public decimal Amount { get; set; }

    public decimal Stake { get; set; }
    public EnumBetStatus BetStatus { get; set; }

    public void ValidateRequest()
    {
        if (string.IsNullOrWhiteSpace(Username))
            throw new Exception("Username is required.");

        if (Amount < 0)
            throw new Exception("Amount must be greater than zero.");

        if (Stake < 0)
            throw new Exception("Stake cannot be negative.");

        if (!Enum.IsDefined(typeof(EnumBetStatus), BetStatus))
            throw new Exception("Invalid BetStatus value.");
    }
}
