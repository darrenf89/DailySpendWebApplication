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

            Bills? B = new();

            return View(B);
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

            if((obj.BillValue < 1 | obj.BillValue > 31) && obj.BillType == "OfEveryMonth")
            {
                ModelState.AddModelError("BillName", "* You have entered an invalid value for your bill. you must enter a day between between 1 and 31");
            }

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            if (ModelState.IsValid)
            {

                decimal DailySavingValue = new();
                TimeSpan Difference = (TimeSpan)(obj.BillDueDate - DateTime.Now);
                int NumberOfDays = Difference.Days;
                decimal BillAmount = obj.BillAmount ?? 0;
                decimal RemainingBillAmount = BillAmount - obj.BillCurrentBalance;
                DailySavingValue = RemainingBillAmount / NumberOfDays;
                DailySavingValue = Math.Round(DailySavingValue, 2);

                B.RegularBillValue = DailySavingValue;
                B.BillName = obj.BillName;
                B.isRecuring = obj.isRecuring;
                B.BillDueDate = obj.BillDueDate;
                B.BillCurrentBalance = obj.BillCurrentBalance;
                B.LastUpdatedDate = DateTime.UtcNow;
                B.BillAmount = obj.BillAmount;

                if (obj.isRecuring)
                {
                    B.BillType = obj.BillType;
                    B.BillValue = obj.BillValue;
                    if (obj.BillType == "Everynth")
                    {
                        B.BillDuration = obj.BillDuration;
                    }
                    else
                    {
                        B.BillDuration = null;
                    }                    
                }

                _db.Attach(Budget);
                Budget.Bills.Add(B);
                _db.SaveChanges();

                ViewBag.PageStatus = "Confirmation";

            }   
            else
            {
                return View(obj);
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBillCancel(Bills obj)
        {
            Budgets? BudgetSavingsList = _db.Budgets?
                .Include(x => x.Bills.Where(x => x.BillName == obj.BillName))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            Bills? CreatedBill = BudgetSavingsList.Bills.First();

            _db.Remove(CreatedBill);
            _db.SaveChangesAsync();

            ViewBag.PageStatus = "Cancel";
            return View("AddBill", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBillConfirmation(Bills obj)
        {

            Budgets? BudgetSavingsList = _db.Budgets?
                .Include(x => x.Bills.Where(x => x.BillName == obj.BillName))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            Bills? CreatedBill = BudgetSavingsList.Bills.First();

            TempData["SnackbarMess"] = "Bill Created";
            return RedirectToAction("Index", "Home", new { ReMess = "BillCreated", id = CreatedBill.BillID });
        }

    }
}
