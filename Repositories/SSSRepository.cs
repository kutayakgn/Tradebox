using CMS.CustomTables;
using CMS.DataEngine;
using Tradebox.Models.ViewModels;

namespace Tradebox.Repositories;

public class SSSRepository : ISSSRepository
{
    private const string SSSTableClassName = "Tradebox.SSSTable";
    private const string CategoriesTableClassName = "Tradebox.SSSCategories";

    private readonly ILogger<SSSRepository> _logger;

    public SSSRepository(ILogger<SSSRepository> logger)
    {
        _logger = logger;
    }

    public List<SSSCategoryViewModel> GetCategories()
    {
        var categories = new List<SSSCategoryViewModel>();

        try
        {
            var items = CustomTableItemProvider.GetItems(CategoriesTableClassName)
                .OrderBy("CategoryName");

            foreach (var item in items)
            {
                categories.Add(new SSSCategoryViewModel
                {
                    CategoryId = item.GetIntegerValue("CategoryValue", 0),
                    CategoryName = item.GetStringValue("CategoryName", string.Empty)
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SSS kategorileri getirilemedi");
        }

        return categories;
    }

    public (List<SSSItemViewModel> Items, int TotalCount) GetItems(
        int? categoryId, string? keyword, int page, int pageSize)
    {
        var result = new List<SSSItemViewModel>();

        try
        {
            // Kategorileri tek seferde çek (join yerine lookup)
            var categoryLookup = GetCategories()
                .ToDictionary(c => c.CategoryId, c => c.CategoryName);

            var query = CustomTableItemProvider.GetItems(SSSTableClassName);

            // Kategori filtresi
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.WhereEquals("Category", categoryId.Value);
            }

            // Anahtar kelime filtresi
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var trimmed = keyword.Trim();
                query = query.Where(w => w
                    .WhereContains("Question", trimmed)
                    .Or()
                    .WhereContains("Answer", trimmed));
            }

            // Toplam kayıt sayısı (pagination hesabı için)
            var totalCount = query.Count;

            // Pagination (ObjectQuery.Page: 0-based page index, pageSize)
            var items = query
                .OrderBy("ItemID")
                .Page(page - 1, pageSize)
                .ToList();

            foreach (var item in items)
            {
                var catId = item.GetIntegerValue("Category", 0);
                categoryLookup.TryGetValue(catId, out var catName);

                result.Add(new SSSItemViewModel
                {
                    ItemID = item.ItemID,
                    Question = item.GetStringValue("Question", string.Empty),
                    Answer = item.GetStringValue("Answer", string.Empty),
                    CategoryId = catId,
                    CategoryName = catName ?? string.Empty
                });
            }

            return (result, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SSS sorulari getirilemedi");
            return (result, 0);
        }
    }
}
