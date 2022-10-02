using Core.Abstractions;
using Core.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Core.Behaviors
{
    public class CachingBehavior<TRequest,TResponse> :IPipelineBehavior<TRequest,TResponse> where TRequest: MediatR.IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;

        public CachingBehavior(IDistributedCache cache,ILogger<TResponse> logger,IOptions<CacheSettings> settings)
        {
          _cache = cache ?? throw new ArgumentNullException(nameof(cache)) ;
          _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          _settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(request is ICacheableMediatrQuery query)
            {
                TResponse response;
                if (query.BypassCache) return await next();
                async Task<TResponse> GetResponseAndAddToCache()
                {
                    response = await next();
                    var slidingExpiration = query.SlidingExpiration == null ? TimeSpan.FromHours(_settings.SlidingExpiration) : query.SlidingExpiration;
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                    var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                    await _cache.SetAsync(query.CacheKey, serializedData, options, cancellationToken);
                    return response;
                }

                var cachedResponse = await _cache.GetAsync(query.CacheKey, cancellationToken);
                if (cachedResponse != null)
                {
                    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                    _logger.LogInformation($"Added to Cache -> '{query.CacheKey}'.");
                }
                else
                {
                    response = await GetResponseAndAddToCache();
                    _logger.LogInformation($"Added to Cache ->'{query.CacheKey}'.");
                }

                return response;
            }
            else
            {
                return await next();
            }

        }
    }
}
