using Microsoft.AspNetCore.Mvc;

namespace DailySpendWebApp.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult AddTransaction()
        {
            TempData["PageHeading"] = "Add a Transaction!";
            return View();
        }
    }
}
