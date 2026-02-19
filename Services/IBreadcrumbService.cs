namespace Tradebox.Services;

public class BreadcrumbItem
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public interface IBreadcrumbService
{
    Task<List<BreadcrumbItem>> GetBreadcrumbsAsync(string nodeAliasPath, string culture = "tr-TR");
}
