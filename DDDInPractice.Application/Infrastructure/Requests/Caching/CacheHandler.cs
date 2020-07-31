using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Caching
{
    public class CacheHandler<TRequest, TResponse> : Handler<TRequest, TResponse> where TRequest: IRequest<TResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheConfig<TRequest> _cacheConfig;

        public CacheHandler(IMemoryCache cache, ICacheConfig<TRequest> cacheConfig)
        {
            _cache = cache;
            _cacheConfig = cacheConfig;
        }

        public override async Task HandleAsync(TRequest request)
        {
            if (!_cacheConfig.IsCacheEnabled)
            {
                await base.HandleAsync(request);
            }
            else
            {
                request.Response = await _cache.GetOrCreateAsync(_cacheConfig.GetCacheKey(request),
                    async entry =>
                    {
                        entry.AbsoluteExpiration = _cacheConfig.CacheDateTimeOffset;
                        await base.HandleAsync(request);
                        return request.Response;
                    });
            }
        }
    }
    
    public interface ICacheConfig<TRequest>
    {
        bool IsCacheEnabled { get; }
        DateTimeOffset CacheDateTimeOffset { get; }
        string GetCacheKey(TRequest request);
    }

    public class CacheConfig<TRequest> : ICacheConfig<TRequest>
    {

        public bool IsCacheEnabled => false;

        public string GetCacheKey(TRequest request)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset CacheDateTimeOffset => default;
    }
}