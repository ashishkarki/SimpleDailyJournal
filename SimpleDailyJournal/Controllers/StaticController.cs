using Microsoft.AspNetCore.Mvc;

namespace SimpleDailyJournal.Controllers;

public class StaticController : Controller
{
    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}