using EarthApi.Caches;
using EarthApi.Models;
using EarthApi.Models.Player;
using EarthApi.Servicies;
using Microsoft.AspNetCore.Mvc;

namespace EarthApi.Controllers
{
    [ApiController]
    public class PlayerController: ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly OnlinePlayerCache _onlinePlayerCache;
        private readonly PlayerBalanceCache _playerBalanceCache;

        public PlayerController(IPlayerService playerService, OnlinePlayerCache onlinePlayerCache, PlayerBalanceCache playerBalanceCache)
        {
            _playerService = playerService;
            _onlinePlayerCache = onlinePlayerCache;
            _playerBalanceCache=playerBalanceCache;
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
    }
}
