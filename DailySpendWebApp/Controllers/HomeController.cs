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
            CultureInfo.DefaultThreadCurrentCulture = _pt.LoadCurrencySetting((int)UserAccount.DefaultBudgetID);
        }

    }

    //[Route("Home/LoadDatePicker/{textElement?}/{InputElement?}/{DatePattern?}/{IncludeDay?}/{Seperator}/{StartDate?}/{EndDate?}/{DefaultDate?}")]
    
    public IActionResult LoadDatePicker(string? textElement, string? InputElement, string? DatePattern, string? IncludeDay, string? Seperator, string? StartDate, string? EndDate, string? DefaultDate)
    {
        DateTime SelectedDate = DateTime.Now;
        DateTime StartDateDP = new DateTime();
        DateTime EndDateDP = DateTime.MaxValue;

        if (DefaultDate != null & DefaultDate != "")
        {
            SelectedDate = DateTime.Parse(DefaultDate);
        }

        if (StartDate != null & StartDate != "")
        {
            StartDateDP = DateTime.Parse(StartDate);
        }


        if (EndDate != null & EndDate != "")
        {
            EndDateDP = DateTime.Parse(EndDate);
        }


        if (SelectedDate < StartDateDP | SelectedDate > EndDateDP)
        {
            StartDateDP = new DateTime();
            EndDateDP = DateTime.MaxValue;
        }

        string Day = SelectedDate.Day.ToString();
        string Month = SelectedDate.Month.ToString();
        string Year = SelectedDate.Year.ToString();


        ViewBag.Day = Day;
        ViewBag.Month = Month;
        ViewBag.Year = Year;

        ViewBag.textElement = textElement;
        ViewBag.InputElement = InputElement;

        ViewBag.StartDate = StartDateDP.ToString("MM/dd/yyyy");
        ViewBag.EndDate = EndDateDP.ToString("MM/dd/yyyy");
        ViewBag.SelectedDate = SelectedDate.ToString("MM/dd/yyyy");

        if (Seperator == "Space")
        {
            Seperator = " ";
        }

        ViewBag.seperator = Seperator;
        ViewBag.DatePattern = DatePattern;
        ViewBag.IncludeDay = IncludeDay; 

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

                obj.NextIncomePayday = Budget.NextIncomePayday ?? DateTime.MinValue;
                //Load Default categories if none exist
                if (Budget.Categories.Count == 0 | Budget.Categories.Count == null)
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
                else if (ReMess == "TransactionCreated")
                {
                    var Transaction = _db.Transactions.Where(x => x.TransactionID == id);

                    Transactions? CreatedTransaction = new();
                    CreatedTransaction = Transaction.FirstOrDefault();

                    string? Amount = String.Format("{0:c}", CreatedTransaction.TransactionAmount);
                     
                    if(!CreatedTransaction.isIncome)
                    {
                        Amount = "-" + Amount;
                    }

                    TempData["TransactionAmount"] = Amount;
                }

                AddUserCurrencyTimeViewBagPreferences();
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

    public IActionResult AddBudgetStatus()
    {
        Budgets obj = _db.Budgets
            .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        return PartialView("_PVLeftToSpendStatus", obj);
    }

    public IActionResult NextPayInfo()
    {
        Budgets obj = _db.Budgets
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        return PartialView("_PVBudgetNextPayInfo", obj);
    }

    public IActionResult RecentActivityTransactions()
    {
        Budgets obj = _db.Budgets
            .Include(x => x.Transactions.OrderByDescending(t => t.TransactionID).Take(10))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        ViewBag.DateString = _pt.GetBudgetDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);

        return PartialView("_PVRecentActivityTransactions", obj);
    }

    public IActionResult RecentActivitySavings()
    {
        Budgets obj = _db.Budgets
            .Include(x => x.Savings.OrderByDescending(s => s.SavingID).Take(5))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        ViewBag.DateString = _pt.GetBudgetDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);

        return PartialView("_PVRecentActivitySavings", obj);
    }

    public IActionResult RecentActivityBills()
    {
        Budgets obj = _db.Budgets
            .Include(x => x.Bills.OrderByDescending(b => b.BillID).Take(5))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        ViewBag.DateString = _pt.GetBudgetDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);

        return PartialView("_PVRecentActivityBills", obj);
    }

    public IActionResult RecentActivityIncomes()
    {
        Budgets obj = _db.Budgets
            .Include(x => x.IncomeEvents.OrderByDescending(i => i.IncomeEventID).Take(5))
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        ViewBag.DateString = _pt.GetBudgetDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);

        return PartialView("_PVRecentActivityIncomes", obj);
    }

    [HttpPost]
    [Route("Home/UpdatePayValue/")]

    public IActionResult UpdatePayValue(decimal? PayAmount)
    {
        Budgets obj = _db.Budgets
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        _db.Attach(obj);

        if (PayAmount != 0 & PayAmount != null)
        {
            obj.PaydayAmount = PayAmount;
        }

        _db.SaveChanges();

        return PartialView("_PVBudgetNextPayInfo", obj);
    }

    [HttpPost]
    public IActionResult UpdatePayDate(DateTime? PayDate)
    {
        Budgets obj = _db.Budgets
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .First();

        _db.Attach(obj);

        if (PayDate != DateTime.MinValue & PayDate != null)
        {
            obj.NextIncomePayday = PayDate;
        }

        _db.SaveChanges();

        return PartialView("_PVBudgetNextPayInfo", obj);
    }

    public void AddUserCurrencyTimeViewBagPreferences()
    {
        ViewBag.CurrencySymbol = CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencySymbol;
        ViewBag.CurrencySpacer = CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyGroupSeparator;
        ViewBag.DecimalSeperator = CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyDecimalSeparator;

        if (CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyPositivePattern == 0)
        {
            ViewBag.CurrencyPlacement = "Before";
            ViewBag.SymbolSpace = "No";
        }
        else if (CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyPositivePattern == 1)
        {
            ViewBag.CurrencyPlacement = "After";
            ViewBag.SymbolSpace = "No";
        }
        else if (CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyPositivePattern == 2)
        {
            ViewBag.CurrencyPlacement = "Before";
            ViewBag.SymbolSpace = "Yes";
        }
        else if (CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencyPositivePattern == 3)
        {
            ViewBag.CurrencyPlacement = "After";
            ViewBag.SymbolSpace = "Yes";
        }

        ViewBag.DateString = _pt.GetBudgetDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);
        ViewBag.ShortDatePattern = CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.ShortDatePattern;
        ViewBag.DateSeperator = CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.DateSeparator;

    }
}
