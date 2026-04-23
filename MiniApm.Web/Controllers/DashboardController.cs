using Microsoft.AspNetCore.Mvc;

namespace MiniApm.Web.Controllers;

public class DashboardController:Controller
{
    public IActionResult Index()
    {
        return View();
    }
}