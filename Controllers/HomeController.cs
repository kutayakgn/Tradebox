using Microsoft.AspNetCore.Mvc;

namespace Tradebox.Controllers;

public class HomeController : Controller
{
    [Route("/Home/Error")]
    public IActionResult Error()
    {
        return View();
    }

    [Route("/Home/NotFound")]
    public IActionResult PageNotFound()
    {
        Response.StatusCode = 404;
        return View("NotFound");
    }
}