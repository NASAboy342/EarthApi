using System;

namespace EarthApi.Models.Player;

public class SettleResponse: EarthApiResponseBase
{
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "";
}
