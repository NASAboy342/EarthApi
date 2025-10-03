using EarthApi.Models.Player;

namespace EarthApi.Servicies
{
    public interface IPlayerService
    {
        bool TryDeductPlayerBalance(string username, decimal amount);
        bool IsPlayerOnlined(string username);
        void LoginPlayer(string username);
        bool TryAddPlayerBalance(string username, decimal amount);
    }
}
