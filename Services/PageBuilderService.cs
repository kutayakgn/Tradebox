using Tradebox.Repositories;
using Tradebox.Extensions;
using Tradebox.Models.ViewModels;

namespace Tradebox.Services;

public class PageBuilderService : IPageBuilderService
{
    private readonly IPageRepository _pageRepository;
    private readonly IComponentTreeBuilder _componentTreeBuilder;
    private readonly ILogger<PageBuilderService> _logger;

    public PageBuilderService(
        IPageRepository pageRepository,
        IComponentTreeBuilder componentTreeBuilder,
        ILogger<PageBuilderService> logger)
    {
        _pageRepository = pageRepository;
        _componentTreeBuilder = componentTreeBuilder;
        _logger = logger;
    }

    public async Task<DynamicPageViewModel?> BuildPageAsync(string url, string culture = "tr-TR")
    {
        var nodeAliasPath = url.ToNodeAliasPath();

        _logger.LogInformation("Sayfa olusturuluyor: {Path}", nodeAliasPath);

        var page = await _pageRepository.GetPageByAliasPathAsync(nodeAliasPath, culture);

        if (page == null)
        {
            _logger.LogWarning("Sayfa bulunamadi: {Path}", nodeAliasPath);
            return null;
        }

        // Recursive component agacini olustur (sonsuz derinlik)
        var componentTree = await _componentTreeBuilder.BuildTreeAsync(nodeAliasPath, culture);

        _logger.LogInformation(
            "Sayfa bulundu: {Name}, Root component sayisi: {Count}",
            page.DocumentName, componentTree.Count);

        return new DynamicPageViewModel
        {
            Page = page,
            ComponentTree = componentTree,
            Meta = new PageMetaViewModel
            {
                Title = page.DocumentName,
                Description = $"{page.DocumentName} sayfasi"
            }
        };
    }
}
