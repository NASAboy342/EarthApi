using EarthApi.Models.Game;
using EarthApi.Models.Player;

namespace EarthApi.Repositories
{
    public interface IEarthRepository
    {
        List<GameInfo> GetAllGames();
        PlayerBalance GetPlayerBalanceByUsername(string username);
        Player GetPlayerByUsername(string username);
    }
}
