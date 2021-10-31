using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Server.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        //throw new NotImplementedException();
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
