using LazyCache;
using Microsoft.Extensions.Logging;
using RoaringAPI.Interface;

namespace RoaringAPI.Service
{
    public class CacheService : ICacheService
    {
        private readonly IAppCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IAppCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public void Add<TItem>(string key, TItem item, TimeSpan duration)
        {
            _cache.Add(key, item, duration);
            _logger.LogInformation($"Added item to cache: {key}");
        }

        public TItem Get<TItem>(string key)
        {
            return _cache.Get<TItem>(key);
        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
