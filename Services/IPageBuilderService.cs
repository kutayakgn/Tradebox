using Tradebox.Models.ViewModels;

namespace Tradebox.Services;

public interface IPageBuilderService
{
    Task<DynamicPageViewModel?> BuildPageAsync(string url, string culture = "tr-TR");
}