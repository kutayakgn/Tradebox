using Tradebox.Models.ViewModels;

namespace Tradebox.Services;

public interface IComponentTreeBuilder
{
    /// <summary>
    /// Verilen parent path altindaki tum component'leri recursive olarak yukler
    /// ve ComponentTreeNode agaci olusturur. Sonsuz derinlik destekler.
    /// </summary>
    /// <param name="parentAliasPath">Ust node'un alias path'i</param>
    /// <param name="culture">Kultur bilgisi</param>
    /// <param name="maxDepth">Maksimum derinlik (guvenlik siniri, varsayilan 10)</param>
    Task<List<ComponentTreeNode>> BuildTreeAsync(
        string parentAliasPath,
        string culture = "tr-TR",
        int maxDepth = 10);
}
