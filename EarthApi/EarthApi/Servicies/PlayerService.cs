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

        public DeductResponse Deduct(DeductRequest request)
        {
            request.ValidateRequest();
            if (!IsPlayerOnlined(request.Username))
                throw new Exception("Player is not online.");

            var success = TryDeductPlayerBalance(request.Username, request.Amount);
            if (!success)
                throw new Exception("Failed to deduct player balance.");

            var playerBalance = _playerBalanceCache.GetByUserName(request.Username);

            var response = new DeductResponse
            {
                Balance = playerBalance.Amount,
                Currency = playerBalance.Currency
            };
            return response;
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

        public SettleResponse Settle(SettleRequest request)
        {
            request.ValidateRequest();
            if (!IsPlayerOnlined(request.Username))
                throw new Exception("Player is not online.");

            var success = TryAddPlayerBalance(request.Username, request.Amount);
            if (!success)
                throw new Exception("Failed to add player balance.");

            var playerBalance = _playerBalanceCache.GetByUserName(request.Username);

            var response = new SettleResponse
            {
                Balance = playerBalance.Amount,
                Currency = playerBalance.Currency
            };
            return response;
        }

        public bool TryAddPlayerBalance(string username, decimal amount)
        {
            var playerBalance = _playerBalanceCache.GetByUserName(username);
            if (playerBalance == null || playerBalance.Amount < amount)
                return false;

            playerBalance.Amount += amount;
            _playerBalanceCache.Set(playerBalance);
            return true;
        }

        public bool TryDeductPlayerBalance(string username, decimal amount)
        {
            var playerBalance = _playerBalanceCache.GetByUserName(username);
            if (playerBalance == null || playerBalance.Amount < amount)
                return false;

            playerBalance.Amount -= amount;
            _playerBalanceCache.Set(playerBalance);
            return true;
        }
    }
}
