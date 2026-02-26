using Microsoft.AspNetCore.Mvc;
using Tradebox.Models.PageTypes;
using Tradebox.Models.ViewModels;
using Tradebox.Repositories;

namespace Tradebox.Components;

public class SSSViewComponent : ViewComponent
{
    private readonly ISSSRepository _sssRepository;

    public SSSViewComponent(ISSSRepository sssRepository)
    {
        _sssRepository = sssRepository;
    }

    public IViewComponentResult Invoke(ComponentTreeNode node)
    {
        var model = (SSS)node.Component;

        // Query string'den filtre ve sayfa bilgisi al
        var request = HttpContext.Request;
        int.TryParse(request.Query["sssPage"], out var currentPage);
        if (currentPage < 1) currentPage = 1;

        int.TryParse(request.Query["sssCategory"], out var categoryId);
        var keyword = request.Query["sssKeyword"].ToString();

        // CMS panelden sabit kategori secilmisse onu baz al,
        // secilmemisse kullanicinin filtre secimini kullan
        var chosenCategory = model.ChosenCategory;
        int? effectiveCategory = chosenCategory > 0
            ? chosenCategory
            : (categoryId > 0 ? categoryId : null);

        // Kategorileri çek
        var categories = _sssRepository.GetCategories();

        // Soruları filtreli + pagination ile çek
        var (items, totalCount) = _sssRepository.GetItems(
            effectiveCategory,
            string.IsNullOrWhiteSpace(keyword) ? null : keyword,
            currentPage,
            model.PageSize);

        var viewModel = new SSSViewModel
        {
            PageSize = model.PageSize,
            IsFilterEnabled = model.IsFilterEnabled,
            ChosenCategory = chosenCategory,
            Items = items,
            Categories = categories,
            CurrentPage = currentPage,
            TotalItems = totalCount,
            SelectedCategoryId = effectiveCategory,
            Keyword = string.IsNullOrWhiteSpace(keyword) ? null : keyword
        };

        return View(viewModel);
    }
}
