using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MobileAppCottage.Application.DTOs;
using MobileAppCottage.Domain.Interfaces;

namespace MobileAppCottage.Application.Services
{
    public class CottageCacheService
    {
        private readonly ICottageRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private const string CottagesCacheKey = "AllCottages";

        public CottageCacheService(ICottageRepository repository, IMemoryCache cache, IMapper mapper)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CottageDto>> GetCachedCottagesAsync()
        {
            // Próbujemy pobrać dane z cache
            if (!_cache.TryGetValue(CottagesCacheKey, out IEnumerable<CottageDto>? cachedCottages))
            {
                // POPRAWIONO: Zmieniono z GetAll() na GetAll()
                // Musi być tak samo jak nazwałeś to w ICottageRepository!
                var cottages = await _repository.GetAll();

                // Mapujemy na DTO
                cachedCottages = _mapper.Map<IEnumerable<CottageDto>>(cottages);

                // Zapisujemy w cache na 10 minut
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(CottagesCacheKey, cachedCottages, cacheOptions);
            }

            return cachedCottages ?? new List<CottageDto>();
        }

        public void ClearCache()
        {
            // Wywołujemy to przy każdej zmianie w bazie (POST/PUT/DELETE)
            _cache.Remove(CottagesCacheKey);
        }
    }
}