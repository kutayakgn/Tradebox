using Tradebox.Models.ViewModels;

namespace Tradebox.Services;

/// <summary>
/// Commons klasorundeki global component'leri (Header, Footer vb.) yukler.
/// Her sayfada gosterilecek ortak component'ler icin kullanilir.
/// </summary>
public interface ICommonsService
{
    Task<List<ComponentTreeNode>> GetCommonsAsync(string culture = "tr-TR");
}
