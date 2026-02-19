using CMS.DocumentEngine;

namespace Tradebox.Repositories;

public interface IPageRepository
{
    Task<TreeNode?> GetPageByAliasPathAsync(string nodeAliasPath, string culture = "tr-TR");
}