using System.Collections.Concurrent;
using CMS.CustomTables;
using CMS.DataEngine;

namespace Tradebox.Services;


public class RedirectService : IRedirectService
{
    private const string CustomTableClassName = "Tradebox.RedirectTable";
    private const string CacheKey = "RedirectService_RedirectMap";

    private static readonly ConcurrentDictionary<string, string> _redirectMap = new(StringComparer.OrdinalIgnoreCase);
    private static DateTime _lastLoadDate = DateTime.MinValue;
    private static readonly object _loadLock = new();

    private readonly ILogger<RedirectService> _logger;

    public RedirectService(ILogger<RedirectService> logger)
    {
        _logger = logger;
    }

    public string? GetRedirectUrl(string fromUrl)
    {
        EnsureCacheLoaded();

        if (string.IsNullOrWhiteSpace(fromUrl))
            return null;

        // Normalize: basta / yoksa ekle
        var normalizedUrl = fromUrl.StartsWith('/') ? fromUrl : "/" + fromUrl;

        if (_redirectMap.TryGetValue(normalizedUrl, out var toUrl))
        {
            _logger.LogInformation("301 Redirect eslesti: {From} -> {To}", normalizedUrl, toUrl);
            return toUrl;
        }

        return null;
    }

    private void EnsureCacheLoaded()
    {
        // Gunluk cache: bugun zaten yuklendiyse tekrar yuklemez
        if (_lastLoadDate.Date == DateTime.Today)
            return;

        lock (_loadLock)
        {
            // Double-check lock icinde
            if (_lastLoadDate.Date == DateTime.Today)
                return;

            LoadRedirectsFromCustomTable();
        }
    }

    private void LoadRedirectsFromCustomTable()
    {
        try
        {
            _logger.LogInformation("Redirect tablosu yukleniyor: {TableName}", CustomTableClassName);

            _redirectMap.Clear();

            var customTableItem = CustomTableItemProvider.GetItems(CustomTableClassName);

            if (customTableItem == null)
            {
                _logger.LogWarning("Custom table bulunamadi: {TableName}", CustomTableClassName);
                _lastLoadDate = DateTime.Today;
                return;
            }

            var count = 0;
            foreach (var item in customTableItem)
            {
                var from = item.GetStringValue("RedirectFrom", string.Empty).Trim();
                var to = item.GetStringValue("RedirectTo", string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                    continue;

                // Normalize
                if (!from.StartsWith('/'))
                    from = "/" + from;

                _redirectMap[from] = to;
                count++;
            }

            _lastLoadDate = DateTime.Today;

            _logger.LogInformation(
                "Redirect tablosu yuklendi: {Count} kural, sonraki yukleme: {NextLoad}",
                count, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redirect tablosu yuklenemedi: {TableName}", CustomTableClassName);
            // Hata durumunda bugun tekrar denenmesin
            _lastLoadDate = DateTime.Today;
        }
    }
}
