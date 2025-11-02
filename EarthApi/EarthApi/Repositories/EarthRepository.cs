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
                    IconUrl = "https://pincloud.ppiinn.net/files/Earth_Games_Icons_/f17b89a4-36ae-4ec2-9e69-0f9665197c55.png",
                    Route = "/bet-n-run2",
                    Description = "Step into the barnyard and test your luck with BetAndRun fast-paced multiplier game where every step holds a chance to multiply your winnings!"
                },
                new GameInfo()
                {
                    Id = 1,
                    Name = "Bet And Run!",
                    IsEnabled = true,
                    IsUnderMaintenance = false,
                    IconUrl = "https://pincloud.ppiinn.net/files/Earth_Games_Icons_/f17b89a4-36ae-4ec2-9e69-0f9665197c55.png",
                    Route = "/bet-n-run2",
                    Description = "Step into the barnyard and test your luck with BetAndRun fast-paced multiplier game where every step holds a chance to multiply your winnings!"
                },
                new GameInfo()
                {
                    Id = 1,
                    Name = "Bet And Run!",
                    IsEnabled = true,
                    IsUnderMaintenance = false,
                    IconUrl = "https://pincloud.ppiinn.net/files/Earth_Games_Icons_/f17b89a4-36ae-4ec2-9e69-0f9665197c55.png",
                    Route = "/bet-n-run2",
                    Description = "Step into the barnyard and test your luck with BetAndRun fast-paced multiplier game where every step holds a chance to multiply your winnings!"
                }
                // new GameInfo()
                // {
                //     Id = 2,
                //     Name = "Drop The Ball",
                //     IsEnabled = true,
                //     IsUnderMaintenance = false,
                //     IconUrl = "",
                //     Route = "/drop-the-ball",
                // }
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
