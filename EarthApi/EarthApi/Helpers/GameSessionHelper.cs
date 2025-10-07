using System;

namespace EarthApi.Helpers;

public class GameSessionHelper
{
    internal static string GenerateSessionId(string gameName)
    {
        return $"{gameName}_{Guid.NewGuid()}";
    }
}
