using DailySpendBudgetWebApp.Controllers;
using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DailySpendWebApp.Controllers
{
    public class BillsController : Controller
    {
        private readonly ILogger<SavingsController> _logger;

        private readonly ApplicationDBContext _db;
        public BillsController(ILogger<SavingsController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult AddBill()
        {

            TempData["NickName"] = HttpContext.Session.GetString("_Name");
            TempData["PageHeading"] = "Add A New Bill!";

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            ViewBag.PaymentPeriod = Budget.AproxDaysBetweenPay;
            ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBill(Bills obj)
        {
            return View(obj);
        }
    }
}
