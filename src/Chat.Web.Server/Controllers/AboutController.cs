using Microsoft.AspNetCore.Mvc;

namespace Chat.Web.Server.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult TellMeMore(string moreInfo = "")
    {
        return new JsonResult(new { name = "TellMeMore", content = moreInfo });
    }
}
