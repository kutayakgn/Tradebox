namespace Tradebox.Models.ViewModels;

public class SSSViewModel
{
    // Component ayarları (Kentico node'undan)
    public int PageSize { get; set; } = 10;
    public bool IsFilterEnabled { get; set; } = true;

    /// <summary>
    /// CMS panelden sabit kategori. 0 ise tum kategoriler, >0 ise sadece o kategori.
    /// </summary>
    public int ChosenCategory { get; set; }

    // Veri
    public List<SSSItemViewModel> Items { get; set; } = new();
    public List<SSSCategoryViewModel> Categories { get; set; } = new();

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int TotalItems { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 1;

    // Filtre durumu
    public int? SelectedCategoryId { get; set; }
    public string? Keyword { get; set; }
}

public class SSSItemViewModel
{
    public int ItemID { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}

public class SSSCategoryViewModel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
