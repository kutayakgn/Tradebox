using CMS.DocumentEngine;

namespace Tradebox.Repositories;

public interface IComponentRepository
{

    Task<List<TreeNode>> GetAllDescendantsAsync(string parentAliasPath, string culture = "tr-TR");
}
