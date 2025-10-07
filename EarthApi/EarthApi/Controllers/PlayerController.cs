using EarthApi.Caches;
using EarthApi.Models;
using EarthApi.Models.Player;
using EarthApi.Servicies;
using Microsoft.AspNetCore.Mvc;

namespace EarthApi.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly OnlinePlayerCache _onlinePlayerCache;
        private readonly PlayerBalanceCache _playerBalanceCache;

        public PlayerController(IPlayerService playerService, OnlinePlayerCache onlinePlayerCache, PlayerBalanceCache playerBalanceCache)
        {
            _playerService = playerService;
            _onlinePlayerCache = onlinePlayerCache;
            _playerBalanceCache = playerBalanceCache;
        }

        [HttpPost("get-player-info")]
        public EarthApiResponse<GetPlayerInfoResponse> GetPlayerInfo([FromBody] GetPlayerInfoRequest request)
        {
            request.ValidateRequest();
            var getPlayerInfoResponse = new GetPlayerInfoResponse();
            if (!_playerService.IsPlayerOnlined(request.Username))
                _playerService.LoginPlayer(request.Username);

            var playerInfo = _onlinePlayerCache.GetByUserName(request.Username);
            var playerBalance = _playerBalanceCache.GetByUserName(request.Username);
            getPlayerInfoResponse.IsOnline = true;
            getPlayerInfoResponse.Username = playerInfo.Username;
            getPlayerInfoResponse.Balance = playerBalance.Amount;
            getPlayerInfoResponse.Currency = playerBalance.Currency;

            return new EarthApiResponse<GetPlayerInfoResponse>(getPlayerInfoResponse);
        }

        [HttpPost("deduct")]
        public EarthApiResponse<DeductResponse> Deduct([FromBody] DeductRequest request)
        {
            var response = _playerService.Deduct(request);

            return new EarthApiResponse<DeductResponse>(response);
        }

        [HttpPost("settle")]
        public EarthApiResponse<SettleResponse> Settle([FromBody] SettleRequest request)
        {
            var response = _playerService.Settle(request);

            return new EarthApiResponse<SettleResponse>(response);
        }

        [HttpPost("sync-balance")]
        public EarthApiResponse<SyncBalanceResponse> SyncBalance([FromBody] SyncBalanceRequest request)
        {
            request.ValidateRequest();
            if (!_playerService.IsPlayerOnlined(request.Username))
                throw new Exception("Player is not online.");

            var playerBalance = _playerBalanceCache.GetByUserName(request.Username);
            if (playerBalance == null)
                throw new Exception("Player balance not found.");

            return new EarthApiResponse<SyncBalanceResponse>(new SyncBalanceResponse
            {
                Balance = playerBalance.Amount,
                Currency = playerBalance.Currency
            });
        }
    }
}
