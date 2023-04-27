using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using DailySpendWebApp.Services;
using System.Globalization;

namespace DailySpendBudgetWebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDBContext _db;

    private readonly IProductTools _pt;

    private readonly IHttpContextAccessor _ca;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext db, IProductTools pt, IHttpContextAccessor ca)
    {
        _logger = logger;
        _db = db;
        _pt = pt;
        _ca = ca;

        string? Email = _ca.HttpContext.User.Identity.Name;
        var Users = _db.UserAccounts
            .Where(x => x.Email == Email);
        UserAccounts? UserAccount = Users.FirstOrDefault();

        if (UserAccount.DefaultBudgetID != null)
        {
            Thread.CurrentThread.CurrentCulture = _pt.LoadCurrencySetting((int)UserAccount.DefaultBudgetID);
        }

    }

    [Route("Home/LoadDatePicker/{textElement?}/{InputElement?}/{DateFormat?}")]
    public IActionResult LoadDatePicker(string? textElement, string? InputElement, string? DateFormat)
    {
        ViewBag.Day = "26";
        ViewBag.Month = "5";
        ViewBag.Year = "2023";

        ViewBag.textElement = textElement;
        ViewBag.InputElement = InputElement;
        ViewBag.StringFormat = DateFormat;

        return PartialView("_PVDatePicker");
    }

    [Route("Home/Index/{id?}")]
    [Route("Home/Index/{id?}/{ReMess?}")]
    public IActionResult Index(int? id, string? ReMess)
    {
        UserHomePageModel obj = new();
        try
        {
            obj.Email = User.FindFirst(ClaimTypes.Name).Value;
            var Users = _db.UserAccounts
                .Where(x => x.Email == obj.Email);
            UserAccounts? UserAccount = Users.FirstOrDefault();
            if (UserAccount.NickName == null || UserAccount.NickName == "")
            {
                obj.NickName = "My Fav User";
            }
            else
            { 
                obj.NickName = UserAccount?.NickName;
            }

            HttpContext.Session.SetString("_Email",UserAccount.Email);
            HttpContext.Session.SetString("_Name", UserAccount.NickName);
            HttpContext.Session.SetInt32("_UserID", UserAccount.UserID);
            

            if (UserAccount.DefaultBudgetID.HasValue)
            {
                HttpContext.Session.SetInt32("_DefaultBudgetID", (int)UserAccount.DefaultBudgetID);
                //Remove after enterbudgetdetailstested
                HttpContext.Session.SetInt32("_NewBudgetID", (int)UserAccount.DefaultBudgetID);
                TempData["PageHeading"] = "Good Morning " + obj.NickName;
                TempData["NickName"] = HttpContext.Session.GetString("_Name");

                Budgets? Budget = _db.Budgets?
                    .Include(b => b.Categories)
                    .Where(b => b.BudgetID == (int)UserAccount.DefaultBudgetID)
                    .FirstOrDefault();

                //Load Default categories if none exist
                if(Budget.Categories.Count == 0 | Budget.Categories.Count == null)
                {
                    _pt.CreateDefaultCategories((int)UserAccount.DefaultBudgetID);
                }

                if (ReMess == "BillCreated")
                {
                    Bills? CreatedBill = new();
                    var Bills = _db.Bills.Where(x => x.BillID == id);
                    CreatedBill = Bills.FirstOrDefault();

                    TempData["BillName"] = CreatedBill.BillName;
                    TempData["BillAmount"] = CreatedBill.BillAmount;
                    TempData["BillDailyValue"] = CreatedBill.RegularBillValue;

                }
                else if (ReMess == "SavingCreated")
                {
                    var Savings = _db.Savings.Where(x => x.SavingID == id);

                    Savings? CreatedSaving = new();
                    CreatedSaving = Savings.FirstOrDefault();

                    TempData["SavingsName"] = CreatedSaving.SavingsName;
                    TempData["SavingsGoal"] = CreatedSaving.SavingsGoal;
                    TempData["RegularSavingValue"] = CreatedSaving.RegularSavingValue;
                }
                else if (ReMess == "EnvolopeSavingCreated")
                {
                    var Savings = _db.Savings.Where(x => x.SavingID == id);

                    Savings? CreatedSaving = new();
                    CreatedSaving = Savings.FirstOrDefault();

                    TempData["SavingsName"] = CreatedSaving.SavingsName;
                    TempData["PeriodSavingValue"] = CreatedSaving.PeriodSavingValue;
                }

                    return View(obj);
            }
            else
            {
                //return View(obj);
                return RedirectToAction("CreateNewBudget", "Budgets");
            }

        }
        catch
        {
            return RedirectToAction("Error", "Shared");
        }


    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AddSavingsTable()
    {
        Budgets? BudgetSavingsList = _db.Budgets?
            .Include(x => x.Savings)
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .FirstOrDefault();

        List<Savings> SavingsList= new List<Savings>();
        foreach(Savings saving in BudgetSavingsList.Savings)
        {
            SavingsList.Add(saving);
        }
        return PartialView("SavingsTableBudget", SavingsList);
    }

}
