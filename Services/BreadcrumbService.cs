using CMS.DocumentEngine;
using Tradebox.Extensions;
using Tradebox.Models.PageTypes;

namespace Tradebox.Services;

public class BreadcrumbService : IBreadcrumbService
{
    private readonly ILogger<BreadcrumbService> _logger;

    private const string HomePath = "/anasayfa";

    public BreadcrumbService(ILogger<BreadcrumbService> logger)
    {
        _logger = logger;
    }

    public Task<List<BreadcrumbItem>> GetBreadcrumbsAsync(string nodeAliasPath, string culture = "tr-TR")
    {
        var breadcrumbs = new List<BreadcrumbItem>();

        try
        {
            // Anasayfa ise breadcrumb gösterme
            if (nodeAliasPath == HomePath || nodeAliasPath == "/")
                return Task.FromResult(breadcrumbs);

            // Yoldaki tüm segment'leri çıkar: /urunler/yardimci-urunler → ["/urunler", "/urunler/yardimci-urunler"]
            var pathSegments = BuildAncestorPaths(nodeAliasPath);

            if (pathSegments.Count == 0)
                return Task.FromResult(breadcrumbs);

            // Tek sorguda tüm ancestor + current page'i çek (typed: Tradebox.Page)
            var pages = DocumentHelper.GetDocuments<Page>()
                .WhereIn("NodeAliasPath", pathSegments.ToArray())
                .Culture(culture)
                .CombineWithDefaultCulture()
                .Published()
                .LatestVersion(false)
                .OnCurrentSite()
                .Columns("NodeID", "NodeAliasPath", "NodeLevel", "PageName")
                .OrderBy("NodeLevel")
                .ToList();

            // NodeAliasPath sırası ile breadcrumb listesi oluştur
            foreach (var path in pathSegments)
            {
                var page = pages.FirstOrDefault(p =>
                    string.Equals(p.NodeAliasPath, path, StringComparison.OrdinalIgnoreCase));

                if (page == null) continue;

                var isActive = string.Equals(path, nodeAliasPath, StringComparison.OrdinalIgnoreCase);
                var url = path == HomePath ? "/" : path.TrimStart('/');

                breadcrumbs.Add(new BreadcrumbItem
                {
                    Title = page.PageName,
                    Url = "/" + url.TrimStart('/'),
                    IsActive = isActive
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Breadcrumb olusturulamadi: {Path}", nodeAliasPath);
        }

        return Task.FromResult(breadcrumbs);
    }

    /// <summary>
    /// /urunler/yardimci-urunler için → ["/urunler", "/urunler/yardimci-urunler"] döner.
    /// /anasayfa ve / segmentleri dahil edilmez.
    /// </summary>
    private static List<string> BuildAncestorPaths(string nodeAliasPath)
    {
        var paths = new List<string>();
        var parts = nodeAliasPath.Trim('/').Split('/');

        var current = string.Empty;
        foreach (var part in parts)
        {
            if (string.IsNullOrWhiteSpace(part)) continue;
            current += "/" + part;

            // Anasayfa segmentini breadcrumb'a ekleme
            if (string.Equals(current, HomePath, StringComparison.OrdinalIgnoreCase))
                continue;

            paths.Add(current);
        }

        return paths;
    }
}
