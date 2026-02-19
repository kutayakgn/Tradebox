using CMS.DocumentEngine;

namespace Tradebox.Models.ViewModels;

public class DynamicPageViewModel
{
    public TreeNode Page { get; set; } = null!;
    /// <summary>
    /// Recursive agac yapisi - sonsuz derinlikte component hiyerarsisi
    /// </summary>
    public List<ComponentTreeNode> ComponentTree { get; set; } = new();
    public PageMetaViewModel Meta { get; set; } = new();
}

public class PageMetaViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string OgImageUrl { get; set; } = string.Empty;
}