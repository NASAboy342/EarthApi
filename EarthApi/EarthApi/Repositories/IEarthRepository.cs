using EarthApi.Models.Player;

namespace EarthApi.Repositories
{
    public interface IEarthRepository
    {
        PlayerBalance GetPlayerBalanceByUsername(string username);
        Player GetPlayerByUsername(string username);
    }
}
