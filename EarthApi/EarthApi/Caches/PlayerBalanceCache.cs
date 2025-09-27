using EarthApi.Models.Player;

namespace EarthApi.Caches
{
    public class PlayerBalanceCache : CacheBase<PlayerBalance>
    {
        private string GetKey(string username) => $"player_balance_{username.ToLower()}";
        public PlayerBalance GetByUserName(string username)
        {
            var key = GetKey(username);
            if (TryGetValue(key, out PlayerBalance? balance))
            {
                return balance!;
            }
            return null!;
        }
        public void Set(PlayerBalance balance)
        {
            var key = GetKey(balance.Username);
            Set(key, balance, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
        }
    }
}
