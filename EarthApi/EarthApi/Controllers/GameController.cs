using EarthApi.Caches;
using EarthApi.Models;
using EarthApi.Models.Game;
using Microsoft.AspNetCore.Mvc;

namespace EarthApi.Controllers
{
    [ApiController]
    public class GameController: ControllerBase
    {
        private readonly GameInfoCache _gameInfoCache;

        public GameController(GameInfoCache gameInfoCache)
        {
            _gameInfoCache = gameInfoCache;
        }

        [HttpGet("get-all-games")]
        public EarthApiResponse<GetAllGamesResponse> GetAllGames()
        {
            var gameInfos = _gameInfoCache.GetAllGames();

            return new EarthApiResponse<GetAllGamesResponse>(new GetAllGamesResponse
            {
                Games = gameInfos
            });
        }
    }
}
