using Microsoft.AspNetCore.Mvc;
using Tradebox.Models.ViewModels;

namespace Tradebox.Components;

/// <summary>
/// CMS.Folder icin ViewComponent. Folder kendisi icerik render etmez,
/// sadece children'i tasir. Eger dogrudan render edilirse children'i gosterir.
/// </summary>
public class FolderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ComponentTreeNode node)
    {
        ViewBag.Children = node.Children;
        return View(node);
    }
}
