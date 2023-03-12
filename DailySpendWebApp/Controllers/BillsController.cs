using DailySpendBudgetWebApp.Controllers;
using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            Bills? B = new();

            var BudgetSavingsList = _db.Budgets?
                .Include(x => x.Bills)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .ToList();

            foreach (var bills in BudgetSavingsList[0].Bills)
            {
                if (bills.BillName == obj.BillName)
                {
                    ModelState.AddModelError("BillName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                    break;
                }
            }

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            if (ModelState.IsValid)
            {

                decimal? DailySavingValue = new();
                TimeSpan Difference = (TimeSpan)(obj.BillDueDate - DateTime.Now);
                int NumberOfDays = Difference.Days;
                decimal? RemainingBillAmount = obj.BillAmount - obj.BillCurrentBalance;
                DailySavingValue = RemainingBillAmount / NumberOfDays;

                B.BillDailyValue = DailySavingValue;
                B.BillName = obj.BillName;
                B.isRecuring = obj.isRecuring;
                B.BillDueDate = obj.BillDueDate;
                B.BillCurrentBalance = obj.BillCurrentBalance;
                B.LastUpdatedDate = DateTime.UtcNow;
                B.LastUpdatedValue = obj.BillCurrentBalance;

                if (obj.isRecuring)
                {
                    B.RecuringPeriod = obj.RecuringPeriod;
                    B.RecuringType = obj.RecuringType;

                }

                _db.Attach(Budget);
                Budget.Bills.Add(B);
                _db.SaveChanges();

                ViewBag.PageStatus = "Confirmation";

            }   
            else
            {

            }

            return View(obj);
        }
    }
}
