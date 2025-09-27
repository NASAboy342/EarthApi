using EarthApi.Models.Player;

namespace EarthApi.Servicies
{
    public interface IPlayerService
    {
        bool IsPlayerOnlined(string username);
        void LoginPlayer(string username);
    }
}
