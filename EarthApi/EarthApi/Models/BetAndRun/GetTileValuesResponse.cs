using System;

namespace EarthApi.Models.BetAndRun;

public class GetTileValuesResponse : EarthApiResponseBase
{
    public List<decimal> TileValues { get; internal set; }
}
