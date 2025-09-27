using EarthApi.Caches;
using EarthApi.Models.Player;
using EarthApi.Repositories;

namespace EarthApi.Servicies
{
    public class PlayerService : IPlayerService
    {
        private readonly OnlinePlayerCache _onlinePlayerCache;
        private readonly PlayerBalanceCache _playerBalanceCache;
        private readonly IEarthRepository _earthRepository;

        public PlayerService(OnlinePlayerCache onlinePlayerCache, PlayerBalanceCache playerBalanceCache, IEarthRepository earthRepository)
        {
            _onlinePlayerCache = onlinePlayerCache;
            _playerBalanceCache=playerBalanceCache;
            _earthRepository=earthRepository;
        }

        public bool IsPlayerOnlined(string username)
        {
            var playerInfo = _onlinePlayerCache.GetByUserName(username);
            return playerInfo != null;
        }

        public void LoginPlayer(string username)
        {
            var playerInfo = _earthRepository.GetPlayerByUsername(username);
            var playerBalance = _earthRepository.GetPlayerBalanceByUsername(username);

            _onlinePlayerCache.Set(playerInfo);
            _playerBalanceCache.Set(playerBalance);
        }
    }
}
