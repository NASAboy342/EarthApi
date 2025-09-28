using EarthApi.Models.Game;
using EarthApi.Models.Player;

namespace EarthApi.Repositories
{
    public class EarthRepository : IEarthRepository
    {
        public List<GameInfo> GetAllGames()
        {
            return new List<GameInfo>
            {
                new GameInfo()
                {
                    Id = 1,
                    Name = "Bet And Run!",
                    IsEnabled = true,
                    IsUnderMaintenance = false,
                    IconUrl = "",
                },
                new GameInfo()
                {
                    Id = 2,
                    Name = "Drop The Ball",
                    IsEnabled = true,
                    IsUnderMaintenance = false,
                    IconUrl = "",
                }
            };
        }

        public PlayerBalance GetPlayerBalanceByUsername(string username)
        {
            return new PlayerBalance
            {
                Username = username,
                Amount = 1000.0m,
                Currency = "USD"
            };
        }

        public Player GetPlayerByUsername(string username)
        {
            return new Player
            {
                DisplayName = "Player_" + username,
                IPAddress = "",
                IsOnline = true,
                LastLogin = DateTime.UtcNow.AddHours(-1),
                LastLogout = DateTime.UtcNow,
                Username = username
            };
        }
    }
}
