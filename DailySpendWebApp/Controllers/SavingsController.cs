using Microsoft.AspNetCore.Mvc;

namespace DailySpendWebApp.Controllers
{
    public class SavingsController : Controller
    {
        public IActionResult AddSaving()
        {
            TempData["NickName"] = HttpContext.Session.GetString("_Name");
            TempData["PageHeading"] = "Add A New Saving!";
            return View();
        }
    }
}
