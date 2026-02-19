using Microsoft.Extensions.Caching.Memory;
using Tradebox.Models.ViewModels;

namespace Tradebox.Services;

/// <summary>
/// CMS'deki /Commons klasoru altindaki global component'leri yukler.
/// Header, Footer gibi her sayfada gosterilecek component'ler burada bulunur.
///
/// MemoryCache kullanir - Kentico CacheMinutes ayarina gore cache'ler.
/// Her request'te DB'ye gitmez, cache suresi dolana kadar memory'den doner.
/// </summary>
public class CommonsService : ICommonsService
{
    private const string CommonsPath = "/Commons";
    private const string CacheKeyPrefix = "Commons_";

    private readonly IComponentTreeBuilder _componentTreeBuilder;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CommonsService> _logger;

    public CommonsService(
        IComponentTreeBuilder componentTreeBuilder,
        IMemoryCache cache,
        IConfiguration configuration,
        ILogger<CommonsService> logger)
    {
        _componentTreeBuilder = componentTreeBuilder;
        _cache = cache;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<ComponentTreeNode>> GetCommonsAsync(string culture = "tr-TR")
    {
        var cacheKey = CacheKeyPrefix + culture;

        // Cache'de varsa direkt don (DB sorgusu yok)
        if (_cache.TryGetValue(cacheKey, out List<ComponentTreeNode>? cached) && cached != null)
        {
            return cached;
        }

        try
        {
            _logger.LogInformation("Commons DB'den yukleniyor: {Path}", CommonsPath);

            var commons = await _componentTreeBuilder.BuildTreeAsync(CommonsPath, culture);

            // Kentico CacheMinutes ayarindan cache suresi al (varsayilan 10 dk)
            var cacheMinutes = _configuration.GetValue("Kentico:CacheMinutes", 10);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheMinutes));

            _cache.Set(cacheKey, commons, cacheOptions);

            _logger.LogInformation(
                "Commons cache'lendi: {Count} adet, Sure: {Minutes} dk",
                commons.Count, cacheMinutes);

            return commons;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Commons yuklenemedi: {Path}", CommonsPath);
            return new List<ComponentTreeNode>();
        }
    }
}
