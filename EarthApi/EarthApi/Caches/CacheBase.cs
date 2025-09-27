using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EarthApi.Caches
{
    public class CacheBase<TCacheObject>
    {
        private readonly MemoryCache _cache;
        public CacheBase()
        {
            _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        }

        public void Set<TCacheObject>(string key, TCacheObject value, MemoryCacheEntryOptions? options = null)
        {
            if (options == null)
            {
                options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
            }
            _cache.Set(key, value, options);
        }

        public bool TryGetValue<TCacheObject>(string key, out TCacheObject? value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
