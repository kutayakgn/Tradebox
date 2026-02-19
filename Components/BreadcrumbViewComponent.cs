using Microsoft.AspNetCore.Mvc;
using Tradebox.Extensions;
using Tradebox.Services;

namespace Tradebox.Components;

public class BreadcrumbViewComponent : ViewComponent
{
    private readonly IBreadcrumbService _breadcrumbService;

    public BreadcrumbViewComponent(IBreadcrumbService breadcrumbService)
    {
        _breadcrumbService = breadcrumbService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string nodeAliasPath)
    {
        var breadcrumbs = await _breadcrumbService.GetBreadcrumbsAsync(nodeAliasPath);
        return View(breadcrumbs);
    }
}
