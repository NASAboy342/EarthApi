using EarthApi.Models.Game;
using EarthApi.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace EarthApi.Caches
{
    public class GameInfoCache : CacheBase<List<GameInfo>>
    {
        private readonly string _key = "GameInfoCacheKey";
        private readonly IEarthRepository _earthRepository;

        public GameInfoCache(IEarthRepository earthRepository)
        {
            _earthRepository = earthRepository;
        }
        public List<GameInfo> GetAllGames()
        {
            if(TryGetValue(_key, out List<GameInfo> gameInfos)){
                return gameInfos;
            }

            ReloadFromDb();

            if(TryGetValue(_key, out gameInfos)){
                return gameInfos;
            }

            throw new Exception("Failed to load game info from database.");
        }

        private void ReloadFromDb()
        {
            var gameInfos = _earthRepository.GetAllGames();
            Set(_key, gameInfos, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
        }
    }
}
