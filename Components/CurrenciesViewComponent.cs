using Microsoft.AspNetCore.Mvc;
using Tradebox.Models.PageTypes;
using Tradebox.Models.ViewModels;

namespace Tradebox.Components;

public class CurrenciesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ComponentTreeNode node)
    {
        var model = (Currencies)node.Component;
        ViewBag.Children = node.Children;
        return View(model);
    }
}
