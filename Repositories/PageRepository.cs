using CMS.DocumentEngine;

namespace Tradebox.Repositories;

public class PageRepository : IPageRepository
{
    private readonly ILogger<PageRepository> _logger;

    public PageRepository(ILogger<PageRepository> logger)
    {
        _logger = logger;
    }

    public Task<TreeNode?> GetPageByAliasPathAsync(string nodeAliasPath, string culture = "tr-TR")
    {
        try
        {
            var document = DocumentHelper.GetDocuments()
                .Path(nodeAliasPath)
                .Culture(culture)
                .CombineWithDefaultCulture()
                .Published()
                .LatestVersion(false)
                .OnCurrentSite()
                .TopN(1)
                .Columns("NodeID", "DocumentName", "NodeAliasPath",
                         "DocumentPageTitle", "DocumentPageDescription",
                         "ClassName", "NodeParentID")
                .FirstOrDefault();

            return Task.FromResult(document);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sayfa getirilemedi: {Path}", nodeAliasPath);
            return Task.FromResult<TreeNode?>(null);
        }
    }

}