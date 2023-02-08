using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;


namespace DailySpendBudgetWebApp.Controllers;

[Authorize]
public class SavingsController : Controller
{
    private readonly ILogger<SavingsController> _logger;

    private readonly ApplicationDBContext _db;
    public SavingsController(ILogger<SavingsController> logger, ApplicationDBContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult AddSaving()
    {

        TempData["NickName"] = HttpContext.Session.GetString("_Name");
        TempData["PageHeading"] = "Add A New Saving!";

        var BudgetList = _db.Budgets
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
        Budgets Budget = BudgetList.FirstOrDefault();

        ViewBag.PaymentPeriod = Budget.AproxDaysBetweenPay;
        ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddSaving(Savings obj, string ReturnUrl)
    {   
        Savings? A = new();

        if (!ModelState.IsValid) 
        {
            var BudgetList = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets Budget = BudgetList.FirstOrDefault();

            var PayPeriod = Budget.AproxDaysBetweenPay;

            A.SavingsName = obj.SavingsName;
            A.LastUpdatedValue = obj.CurrentBalance;
            A.LastUpdatedDate = DateTime.Now;
            A.SavingsGoal = obj.SavingsGoal;

            if (obj.isRegularSaving == false)
            {
                A.isRegularSaving = false;
                A.CurrentBalance = obj.CurrentBalance;
                A.canExceedGoal = true;
                A.isAutoComplete = false;
                A.isDailySaving = false;
                
            }
            else if (obj.isRegularSaving == true)
            {
                A.isRegularSaving = true;
                A.isDailySaving = true;
                A.CurrentBalance = obj.CurrentBalance;

                if (obj.SavingsType == "TargetDate")
                {

                    A.SavingsType = obj.SavingsType;                
                    A.GoalDate = obj.GoalDate;
                    A.RegularSavingValue= obj.RegularSavingValue;
                    A.canExceedGoal = obj.canExceedGoal;
                    A.isAutoComplete = false;

                }
                else if (obj.SavingsType == "SavingsBuilder")
                {
                    A.SavingsType = obj.SavingsType;                
                    A.RegularSavingValue= obj.RegularSavingValue;
                    A.SavingsGoal = null;
                    A.canExceedGoal = false;
                    A.isAutoComplete = false;

                }
                else if (obj.SavingsType == "TargetAmount")
                {

                    A.SavingsType = obj.SavingsType;
                    A.SavingsGoal = obj.SavingsGoal;
                    A.CurrentBalance = obj.CurrentBalance;
                    A.GoalDate = obj.GoalDate;
                    A.RegularSavingValue = obj.RegularSavingValue;
                    A.canExceedGoal = obj.canExceedGoal;
                    A.isAutoComplete = obj.isAutoComplete;
                    A.isSavingsClosed = false;
                    A.isRegularSaving = true;
                    A.isDailySaving = false;
                }
                else
                {
                    ModelState.AddModelError("SavingsType", "* There is a probelm, we don't have a clue the type of saving. Could be your fault, could be ours but you sort it out.");
                }
            }
        }

        return View(obj);
    }
}

