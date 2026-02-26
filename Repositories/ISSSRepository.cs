using Tradebox.Models.ViewModels;

namespace Tradebox.Repositories;

public interface ISSSRepository
{
    List<SSSCategoryViewModel> GetCategories();
    (List<SSSItemViewModel> Items, int TotalCount) GetItems(int? categoryId, string? keyword, int page, int pageSize);
}
