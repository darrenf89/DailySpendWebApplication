using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

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

        var BudgetList = _db.Budgets?
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
        Budgets? Budget = BudgetList?.FirstOrDefault();

        ViewBag.PaymentPeriod = Budget.AproxDaysBetweenPay;
        ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddSaving(Savings obj)
    {   
        Savings? S = new();

        var BudgetSavingsList = _db.Budgets?
            .Include(x=>x.Savings)
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .ToList();

        foreach (var saving in BudgetSavingsList[0].Savings)
        {
            if (saving.SavingsName == obj.SavingsName && !saving.isSavingsClosed)
            {
                ModelState.AddModelError("SavingsName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                break;
            }
        }

        var BudgetList = _db.Budgets?
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
        Budgets? Budget = BudgetList?.FirstOrDefault();

        if (ModelState.IsValid) 
        {
            S.SavingsName = obj.SavingsName;
            S.LastUpdatedValue = obj.CurrentBalance;
            S.LastUpdatedDate = DateTime.Now;            

            if (obj.isRegularSaving == false)
            {
                S.isRegularSaving = false;
                S.GoalDate = Budget.NextIncomePayday;
                S.CurrentBalance = obj.CurrentBalance;
                S.canExceedGoal = true;
                S.isAutoComplete = false;
                S.isDailySaving = false;
                S.PeriodSavingValue = obj.RegularSavingValue;
                S.RegularSavingValue = null;
                S.ddlSavingsPeriod = "PerPayPeriod";

            }
            else if (obj.isRegularSaving == true)
            {
                S.isRegularSaving = true;                
                S.CurrentBalance = obj.CurrentBalance;
                S.SavingsGoal = obj.SavingsGoal;

                if (obj.SavingsType == "TargetDate")
                {

                    S.SavingsType = obj.SavingsType;                
                    S.GoalDate = obj.GoalDate;
                    S.RegularSavingValue= obj.RegularSavingValue;
                    S.ddlSavingsPeriod = "PerDay";
                    S.PeriodSavingValue = null;
                    S.canExceedGoal = obj.canExceedGoal;
                    S.isAutoComplete = false;
                    S.isDailySaving = true;

                }
                else if (obj.SavingsType == "SavingsBuilder")
                {
                    S.SavingsType = obj.SavingsType;                
                    S.SavingsGoal = null;
                    S.canExceedGoal = false;
                    S.isAutoComplete = false;
                    S.ddlSavingsPeriod = obj.ddlSavingsPeriod;
                    if (obj.ddlSavingsPeriod == "PerDay")
                    {
                        S.RegularSavingValue = obj.RegularSavingValue;
                        S.PeriodSavingValue = null;
                        S.isDailySaving = true;
                    }
                    else if (obj.ddlSavingsPeriod == "PerPayPeriod")
                    {
                        S.PeriodSavingValue = obj.RegularSavingValue;
                        S.RegularSavingValue = obj.RegularSavingValue / Budget.AproxDaysBetweenPay;
                        S.isDailySaving = false;
                    }

                }
                else if (obj.SavingsType == "TargetAmount")
                {

                    S.SavingsType = obj.SavingsType;
                    S.SavingsGoal = obj.SavingsGoal;
                    S.CurrentBalance = obj.CurrentBalance;
                    S.GoalDate = obj.GoalDate;
                    S.canExceedGoal = obj.canExceedGoal;
                    S.isAutoComplete = obj.isAutoComplete;
                    S.isSavingsClosed = false;
                    S.isRegularSaving = true;
                    S.ddlSavingsPeriod = obj.ddlSavingsPeriod;
                    if (obj.ddlSavingsPeriod == "PerDay")
                    {
                        S.RegularSavingValue = obj.RegularSavingValue;
                        S.PeriodSavingValue = null;
                        S.isDailySaving = true;
                    }
                    else if (obj.ddlSavingsPeriod == "PerPayPeriod")
                    {
                        S.PeriodSavingValue = obj.RegularSavingValue;
                        S.RegularSavingValue = obj.RegularSavingValue / Budget.AproxDaysBetweenPay;
                        S.isDailySaving = false;
                    }
                }
                else
                {
                    ModelState.AddModelError("SavingsType", "* There is a probelm, we don't have a clue what type of saving you want. Could be your fault, could be ours but you sort it out.");
                }
            }

            _db.Attach(Budget);
            Budget.Savings.Add(S);
            _db.SaveChanges();

        }
        else
        {
            if (obj.isRegularSaving== false)
            {
                ViewBag.PageStatus = "Savings Name Error NotRegular";
            }
            else
            {
                ViewBag.PageStatus = "Savings Name Error Regular";
            }
            ViewBag.PaymentPeriod = Budget?.AproxDaysBetweenPay;
            return View();
        }

        ViewBag.isRegularSaving = S.isRegularSaving;
        ViewBag.GoalDate = S.GoalDate;
        ViewBag.SavingsName = S.SavingsName;
        ViewBag.SavingsGoal = S.SavingsGoal;
        ViewBag.RegularSavingValue = S.RegularSavingValue;
        ViewBag.SavingsType = S.SavingsType;
        ViewBag.PageStatus = "Confirmation";
        ViewBag.PaymentPeriod = Budget.AproxDaysBetweenPay;
        ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddSavingCancel(Savings obj)
    {
        Budgets? BudgetSavingsList = _db.Budgets?
            .Include(x=>x.Savings.Where(x=>x.SavingsName == obj.SavingsName))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .FirstOrDefault();

        Savings? CreatedSaving = BudgetSavingsList.Savings.First();

        _db.Remove(CreatedSaving);
        _db.SaveChangesAsync();

        ViewBag.PageStatus = "Cancel";
        return View("AddSaving", obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddSavingConfirmation(Savings obj)
    {

        Budgets? BudgetSavingsList = _db.Budgets?
            .Include(x => x.Savings.Where(x => x.SavingsName == obj.SavingsName))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .FirstOrDefault();

        Savings? CreatedSaving = BudgetSavingsList.Savings.First();

        ViewBag.CurrentDate = (DateTime.Today).AddDays(1);
        TempData["SnackbarMess"] = "SavingCreated";
        return RedirectToAction("Index", "Home", new {ReMess = "SavingCreated", id = CreatedSaving.SavingID});
    }



}
