using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using DailySpendBudgetWebApp.Controllers;

namespace DailySpendWebApp.Controllers
{
    public class IncomesController : Controller
    {
        private readonly ILogger<IncomesController> _logger;

        private readonly ApplicationDBContext _db;
        public IncomesController(ILogger<IncomesController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult AddIncome()
        {
            TempData["NickName"] = HttpContext.Session.GetString("_Name");
            TempData["PageHeading"] = "Add A New Income!";

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

            IncomeEvents? I = new();

            return View(I);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIncome(IncomeEvents obj)
        {
            IncomeEvents? I = new();

            var BudgetSavingsList = _db.Budgets?
                .Include(x => x.IncomeEvents)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .ToList();

            if(obj.isInstantActive == null)
            {
                ModelState.AddModelError("IncomeName", "* You have to tell us when you want the income to be active.");
            }

            foreach (var Income in BudgetSavingsList[0].IncomeEvents)
            {
                if (Income.IncomeName == obj.IncomeName)
                {
                    ModelState.AddModelError("IncomeName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                    break;
                }
            }

            if ((obj.RecurringIncomeValue < 1 | obj.RecurringIncomeValue > 31) && obj.RecurringIncomeType == "OfEveryMonth")
            {
                ModelState.AddModelError("IncomeName", "* You have entered an invalid value for your Income. you must enter a day between between 1 and 31");
            }

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            if (ModelState.IsValid)
            {
                I.IncomeAmount = obj.IncomeAmount;
                I.DateOfIncomeEvent = obj.DateOfIncomeEvent;
                I.IncomeName = obj.IncomeName;
                I.isClosed = false;
                I.isRecurringIncome = obj.isRecurringIncome;
                I.isInstantActive = obj.isInstantActive;

                if(obj.isInstantActive ?? false)
                {
                    I.IncomeActiveDate = obj.IncomeActiveDate;
                }
                else
                {
                    I.IncomeActiveDate = obj.DateOfIncomeEvent;
                }

                if (obj.isRecurringIncome)
                {
                    I.RecurringIncomeType = obj.RecurringIncomeType;
                    I.RecurringIncomeValue = obj.RecurringIncomeValue;
                    if (obj.RecurringIncomeType == "Everynth")
                    {
                        I.RecurringIncomeDuration = obj.RecurringIncomeDuration;
                    }
                    else
                    {
                        I.RecurringIncomeDuration = null;
                    }
                }

                _db.Attach(Budget);
                Budget.IncomeEvents.Add(I);
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
        public IActionResult AddIncomeCancel(IncomeEvents obj)
        {
            Budgets? BudgetSavingsList = _db.Budgets?
                .Include(x => x.IncomeEvents.Where(x => x.IncomeName == obj.IncomeName))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            IncomeEvents? CreatedIncome = BudgetSavingsList.IncomeEvents.First();

            _db.Remove(CreatedIncome);
            _db.SaveChangesAsync();

            ViewBag.PageStatus = "Cancel";
            return View("AddIncome", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIncomeConfirmation(IncomeEvents obj)
        {

            Budgets? BudgetSavingsList = _db.Budgets?
                .Include(x => x.IncomeEvents.Where(x => x.IncomeName == obj.IncomeName))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            IncomeEvents? CreatedIncome = BudgetSavingsList.IncomeEvents.First();

            TempData["SnackbarMess"] = "Bill Created";
            return RedirectToAction("Index", "Home", new {id = CreatedIncome.IncomeEventID, ReMess = "IncomeCreated" });
        }

    }
}

