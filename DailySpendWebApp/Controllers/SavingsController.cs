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
            if (obj.SavingsType == "TargetDate")
            {
                A.SavingsName = obj.SavingsName;
                A.SavingsType = obj.SavingsType;                
                A.SavingsGoal = obj.SavingsGoal;
                A.CurrentBalance = obj.CurrentBalance;
                A.GoalDate = obj.GoalDate;
                A.RegularSavingValue= obj.RegularSavingValue;
                A.canExceedGoal = obj.canExceedGoal;
                A.isAutoComplete = false;
                A.isSavingsClosed = false;
                A.isRegularSaving = true;
                A.isDailySaving = true;

            }
            else if (obj.SavingsType == "SavingsBuilder")
            {
                A.SavingsName = obj.SavingsName;
                A.SavingsType = obj.SavingsType;
                A.SavingsGoal = obj.SavingsGoal;
                A.CurrentBalance = obj.CurrentBalance;
                A.GoalDate = obj.GoalDate;
                A.RegularSavingValue = obj.RegularSavingValue;
                A.canExceedGoal = false;
                A.isAutoComplete = false;
                A.isSavingsClosed = false;
                A.isRegularSaving = true;
                A.isDailySaving = false;

            }
            else if (obj.SavingsType == "TargetAmount")
            {
                A.SavingsName = obj.SavingsName;
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

        return View(obj);
    }
}

