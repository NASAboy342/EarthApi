using EarthApi.Models.Player;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EarthApi.Caches
{
    public class OnlinePlayerCache : CacheBase<Player>
    {
        public OnlinePlayerCache()
        {
        }

        private string GetKey(string username) => $"online_player_{username.ToLower()}";

        public void Set(Player player)
        {
            var key = GetKey(player.Username);
            base.Set(key, player, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
        }

        public Player GetByUserName(string username)
        {
            var key = GetKey(username);
            if (TryGetValue(key, out Player? player))
            {
                return player!;
            }
            return null!;
        }

        public bool IsPlayerOnlined(string username, out Player? player)
        {
            var key = GetKey(username);
            return TryGetValue(key, out player);
        }

        public void ExitGameSession(string username)
        {
            var player = GetByUserName(username);
            if (TryGetValue(GetKey(username), out Player? playerInfo))
            {
                playerInfo.GameSessionInJson = null;
                Set(playerInfo);
                return;
            }
            throw new Exception("Player is not online.");
        }
    }
}
