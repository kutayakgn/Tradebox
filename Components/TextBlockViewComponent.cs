using Microsoft.AspNetCore.Mvc;
using Tradebox.Models.PageTypes;
using Tradebox.Models.ViewModels;

namespace Tradebox.Components;

public class TextBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ComponentTreeNode node)
    {
        var model = (TextBlock)node.Component;
        ViewBag.Children = node.Children;
        return View(model);
    }
}
