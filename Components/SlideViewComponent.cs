using Microsoft.AspNetCore.Mvc;
using Tradebox.Models.PageTypes;
using Tradebox.Models.ViewModels;

namespace Tradebox.Components;

public class SlideViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ComponentTreeNode node)
    {
        var model = (Slide)node.Component;
        ViewBag.Children = node.Children;
        return View(model);
    }
}
