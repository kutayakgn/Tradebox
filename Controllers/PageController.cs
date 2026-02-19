using Microsoft.AspNetCore.Mvc;
using Tradebox.Services;

namespace Tradebox.Controllers;

public class PageController : Controller
{
    private readonly IPageBuilderService _pageBuilderService;
    private readonly ILogger<PageController> _logger;

    public PageController(
        IPageBuilderService pageBuilderService,
        ILogger<PageController> logger)
    {
        _pageBuilderService = pageBuilderService;
        _logger = logger;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        return await RenderPage("/anasayfa");
    }

    [Route("/{*url}")]
    public async Task<IActionResult> DynamicPage(string url)
    {
        return await RenderPage(url);
    }

    private async Task<IActionResult> RenderPage(string url)
    {
        var viewModel = await _pageBuilderService.BuildPageAsync(url);

        if (viewModel == null)
            return View("NotFound");

        return View("DynamicPage", viewModel);
    }
}
