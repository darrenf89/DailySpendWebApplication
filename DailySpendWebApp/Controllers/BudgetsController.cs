
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DailySpendBudgetWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DailySpendBudgetWebApp.Migrations;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Data.SqlClient.Server;

namespace DailySpendBudgetWebApp.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDBContext _db;

        public BudgetsController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public CreateABudgetPageModel ValidateBudgetDetails(CreateABudgetPageModel obj)
        {
            DateTime PayDate = new DateTime();
            bool Everynth = obj.Everynth ?? false;
            bool WorkingDays = obj.WorkingDays ?? false;
            bool OfEveryMonth = obj.OfEveryMonth ?? false;
            bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

            if (obj.StartingBalance == "" | obj.StartingBalance == null)
            {
                ModelState.AddModelError("StartingBalance", "* You have to enter your current balance, or you'll have no money to spend!");
            }

            if (obj.NextPayDayAmount == "" | obj.NextPayDayAmount == null)
            {
                ModelState.AddModelError("NextPayDayAmount", "* Tell us roughly how much you get paid, you can edit it later if it changes so don't worry!");
            }

            if (obj.NextIncomeDate == "" | obj.NextIncomeDate == null)
            {
                ModelState.AddModelError("NextIncomeDate", "* Please select a date for your next pay day, and make sure it is in the future... obviously!");
            }
            else
            {
                Regex regex = new Regex("[0-9]{4}-[0-9]{2}-[0-9]{2}", RegexOptions.IgnoreCase);
                if (!(regex.IsMatch(obj.NextIncomeDate)))
                {
                    ModelState.AddModelError("NextIncomeDate", "* Invalid Date Format... Please Check and try Again!");
                }
                else
                {
                    string format = "yyyy-MM-dd";
                    if (!(DateTime.TryParseExact(obj.NextIncomeDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out PayDate)))
                    {
                        ModelState.AddModelError("NextIncomeDate", "* Invalid Date Format... Please Check and try Again!");
                    }
                    else
                    {
                        if (DateTime.UtcNow.AddDays(1) >= PayDate)
                        {
                            ModelState.AddModelError("NextIncomeDate", "* Please add a date in the future .. you know your next pay day!");
                        }
                    }
                }
            }

            if (Everynth)
            {
                if (obj.PeriodicPayPeriod == null | obj.PeriodicPayPeriod == 0 | obj.PeriodicPayPeriodDDL == null | obj.PeriodicPayPeriodDDL == "")
                {
                    ModelState.AddModelError("LastGivenDayOfWeekPay", "* You either haven't enetered or entered incorrect details please check again!");
                }
            }
            else if (WorkingDays)
            {
                if (obj.LastDayOfMonthPayPeriod == null | obj.LastDayOfMonthPayPeriod == 0)
                {
                    ModelState.AddModelError("LastGivenDayOfWeekPay", "* You either haven't enetered or entered incorrect details please check again!");
                }
            }
            else if (OfEveryMonth)
            {
                if (obj.GivenDayOfMonthPayPeriod == null | obj.GivenDayOfMonthPayPeriod == 0 | obj.GivenDayOfMonthPayPeriod > 28)
                {
                    ModelState.AddModelError("LastGivenDayOfWeekPay", "* You either haven't enetered or entered incorrect details please check again!");
                }
            }
            else if (LastOfTheMonth)
            {
                if (obj.LastGivenDayOfWeekPay == null | obj.LastGivenDayOfWeekPay == "")
                {
                    ModelState.AddModelError("LastGivenDayOfWeekPay", "* You either haven't enetered or entered incorrect details please check again!");
                }
            }
            else
            {
                ModelState.AddModelError("LastGivenDayOfWeekPay", "* Please select at least one type to tell us how often you get paid!");
            }

            return obj;
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBudgetDetailsStage(CreateABudgetPageModel obj)
        {
            obj = UpdateEnterBudgetDetailsPageFromModel(obj, "EditBD");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBudgetSettingsStage(CreateABudgetPageModel obj)
        {
            obj = UpdateEnterBudgetDetailsPageFromModel(obj, "EditBS");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBudgetSettings(CreateABudgetPageModel obj)
        {

            if (ModelState.IsValid)
            {              

                var BudgetsSetting = _db.BudgetSettings?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

                BudgetSettings? BS = BudgetsSetting.FirstOrDefault();

                if (BS == null)
                {
                    BS = new BudgetSettings();
                    _db.BudgetSettings.Add(BS);
                    BS.BudgetID = HttpContext.Session.GetInt32("_DefaultBudgetID");
                }

                BS.CurrencySymbol = int.Parse(obj.CurrencyDDL);
                BS.CurrencyPattern = int.Parse(obj.CurrencyPlacementDDL);

                int DateFormatObj = int.Parse(obj.DateFormatDDL);
                var DateFormats = _db.lut_DateFormats.Where(x => x.id == DateFormatObj);
                lut_DateFormat DateFormat = DateFormats.FirstOrDefault();

                BS.DateSeperator = DateFormat.DateSeperatorID;
                BS.ShortDatePattern = DateFormat.ShortDatePatternID;

                int NumberFormatObj = int.Parse(obj.NumberFormatDDL);
                var NumberFormats = _db.lut_NumberFormats.Where(x => x.id == NumberFormatObj);
                lut_NumberFormat NumberFormat = NumberFormats.FirstOrDefault();

                BS.CurrencyDecimalDigits = NumberFormat.CurrencyDecimalDigitsID;
                BS.CurrencyDecimalSeparator = NumberFormat.CurrencyDecimalSeparatorID;
                BS.CurrencyGroupSeparator = NumberFormat.CurrencyGroupSeparatorID;


                _db.SaveChanges();
                obj = UpdateEnterBudgetDetailsPageFromModel(obj, "SaveBS");
            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj, "Error");
            }

            return View("EnterBudgetDetails", obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBudgetDetails(CreateABudgetPageModel obj)
        {
            obj = ValidateBudgetDetails(obj);            

            if (ModelState.IsValid)
            {
                
                bool Everynth = obj.Everynth ?? false;
                bool WorkingDays = obj.WorkingDays ?? false;
                bool OfEveryMonth = obj.OfEveryMonth ?? false;
                bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

                var Budgets = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

                Budgets? Budget = Budgets.FirstOrDefault();

                Decimal Balance;
                Decimal.TryParse(obj.StartingBalance, out Balance);

                Budget.BankBalance = Balance;
                Budget.MoneyAvailableBalance = Balance;
                Budget.LeftToSpendBalance = Balance;

                Decimal Pay;
                Decimal.TryParse(obj.NextPayDayAmount, out Pay);

                Budget.PaydayAmount = Pay;

                string format = "yyyy-MM-dd";
                DateTime PayDate = new DateTime();
                DateTime.TryParseExact(obj.NextIncomeDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out PayDate);
                Budget.NextIncomePayday = PayDate;

                if (Everynth)
                {
                    Budget.PaydayType = "Everynth";
                    Budget.PaydayValue = obj.PeriodicPayPeriod;
                    Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                }
                else if (WorkingDays)
                {
                    Budget.PaydayType = "WorkingDays";
                    Budget.PaydayValue = obj.LastDayOfMonthPayPeriod;
                    Budget.PaydayDuration = null;
                }
                else if (OfEveryMonth)
                {
                    Budget.PaydayType = "OfEveryMonth";
                    Budget.PaydayValue = obj.GivenDayOfMonthPayPeriod;
                    Budget.PaydayDuration = null;
                }
                else if (LastOfTheMonth)
                {
                    Budget.PaydayType = "LastOfTheMonth";
                    Budget.PaydayValue = null;
                    Budget.PaydayDuration = obj.LastGivenDayOfWeekPay;
                }

                Budget.BudgetValuesLastUpdated = DateTime.UtcNow;

                TimeSpan t = Budget.NextIncomePayday - DateTime.UtcNow ?? new TimeSpan();
                Budget.AproxDaysBetweenPay = t.Days;
                Budget.LeftToSpendDailyAmount = Balance / t.Days;


                _db.SaveChanges();
                obj = UpdateEnterBudgetDetailsPageFromModel(obj, "SaveBD");
            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj, "Error");
            }
                        
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSavingBudget(Savings obj)
        {
            Savings? S = new();

            var BudgetSavingsList = _db.Budgets?
                .Include(x => x.Savings)
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
                S.SavingsGoal = obj.SavingsGoal;

                if (obj.isRegularSaving == false)
                {
                    S.isRegularSaving = false;
                    S.CurrentBalance = obj.CurrentBalance;
                    S.canExceedGoal = true;
                    S.isAutoComplete = false;
                    S.isDailySaving = false;

                }
                else if (obj.isRegularSaving == true)
                {
                    S.isRegularSaving = true;
                    S.isDailySaving = true;
                    S.CurrentBalance = obj.CurrentBalance;

                    if (obj.SavingsType == "TargetDate")
                    {

                        S.SavingsType = obj.SavingsType;
                        S.GoalDate = obj.GoalDate;
                        S.RegularSavingValue = obj.RegularSavingValue;
                        S.canExceedGoal = obj.canExceedGoal;
                        S.isAutoComplete = false;

                    }
                    else if (obj.SavingsType == "SavingsBuilder")
                    {
                        S.SavingsType = obj.SavingsType;
                        S.RegularSavingValue = obj.RegularSavingValue;
                        S.SavingsGoal = null;
                        S.canExceedGoal = false;
                        S.isAutoComplete = false;

                    }
                    else if (obj.SavingsType == "TargetAmount")
                    {

                        S.SavingsType = obj.SavingsType;
                        S.SavingsGoal = obj.SavingsGoal;
                        S.CurrentBalance = obj.CurrentBalance;
                        S.GoalDate = obj.GoalDate;
                        S.RegularSavingValue = obj.RegularSavingValue;
                        S.canExceedGoal = obj.canExceedGoal;
                        S.isAutoComplete = obj.isAutoComplete;
                        S.isSavingsClosed = false;
                        S.isRegularSaving = true;
                        S.isDailySaving = false;
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
                if (obj.isRegularSaving == false)
                {
                    ViewBag.PageStatus = "Savings Name Error NotRegular";
                }
                else
                {
                    ViewBag.PageStatus = "Savings Name Error Regular";
                }

                return View();
            }

            ViewBag.PageStatus = "Confirmation";

            return RedirectToAction("EnterBudgetDetails");
        }

        public IActionResult AddSavingPV()
        {
            return PartialView("_SavingsModalPV");
        }

        public IActionResult CreateNewBudget()
        {
            return View();
        }

        public IActionResult EnterBudgetDetails(CreateABudgetPageModel obj)
        {
            if (HttpContext.Session.GetInt32("_NewBudgetID") != null)
            {

                obj = UpdateEnterBudgetDetailsPageFromModel(obj, "Initial");
                return View(obj);
            }
            else
            {
                return RedirectToAction("Error", "Shared");
            }            
        }

        [Route("Budgets/EditBudgetDetails/{id?}")]
        public IActionResult EditBudgetDetails(int? id)
        {
            TempData["NickName"] = HttpContext.Session.GetString("_Name");
            TempData["PageHeading"] = "Update your Budget!";

            if (id != null)
            {
                ViewBag.hasBudgetName = true;
                var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("id"));
                Budgets Budget = BudgetList.FirstOrDefault();
                ViewBag.BudgetName = Budget.BudgetName;
            }
            else if (HttpContext.Session.GetInt32("_DefaultBudgetID") != null)
            {
                //Get the DefaultBudgetID and the Budget Setting and prefill

                ViewBag.hasBudgetName = true;
                var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
                Budgets Budget = BudgetList.FirstOrDefault();
                ViewBag.BudgetName = Budget.BudgetName;
            }
            else
            {
                //Error Path
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewBudget(CreateABudgetPageModel obj)
        {
            if (ModelState.IsValid)
            {
                int UserID = (int)HttpContext.Session.GetInt32("_UserID");
                var Users = _db.UserAccounts
                    .Where(x => x.UserID == UserID);
                UserAccounts? UserAccount = Users.FirstOrDefault();


                Budgets? B = new();
                B.BudgetName = obj.BudgetName;
                B.BudgetCreatedOn = DateTime.UtcNow;

                _db.Attach(UserAccount);
                UserAccount.Budgets.Add(B);
                _db.SaveChanges();

                HttpContext.Session.SetInt32("_NewBudgetID", B.BudgetID);
                HttpContext.Session.SetString("_NewBudgetName", B.BudgetName);
                UserAccount.DefaultBudgetID = B.BudgetID;
                obj.BudgetID = B.BudgetID;

                _db.Update(UserAccount);
                _db.SaveChanges();

                return RedirectToAction("EnterBudgetDetails", obj);
            }
            else
            {
                return View(obj);
            }
        }

        public CreateABudgetPageModel UpdateEnterBudgetDetailsPageFromModel(CreateABudgetPageModel obj, string Stage)
        {
            var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
            Budgets Budget = BudgetList.FirstOrDefault();

            ViewBag.PaymentPeriod = Budget?.AproxDaysBetweenPay;
            ViewBag.hasBudgetName = true;
            ViewBag.BudgetName = Budget?.BudgetName;

            if (obj.NextIncomeDate == null)
            {
                if (Budget.NextIncomePayday != null)
                {
                    DateTime dt = Budget.NextIncomePayday.GetValueOrDefault();
                    obj.NextIncomeDate = dt.ToString("yyyy-MM-dd");
                }
                else
                {
                    obj.NextIncomeDate = null;
                }
            }

            if (obj.StartingBalance == null)
            {
                if (Budget.BankBalance != null & Budget.BankBalance != 0)
                {
                    obj.StartingBalance = String.Format("{0}", Budget.BankBalance);
                }
                else
                {
                    obj.StartingBalance = null;
                }
            }

            if (obj.NextPayDayAmount == null)
            {
                if (Budget.PaydayAmount != null & Budget.PaydayAmount != 0)
                {
                    obj.NextPayDayAmount = String.Format("{0}", Budget.PaydayAmount);
                }
                else
                {
                    obj.NextPayDayAmount = null;
                }
            }

            if (obj.PeriodicPayPeriod == null & obj.PeriodicPayPeriodDDL == null & obj.LastDayOfMonthPayPeriod == null & obj.GivenDayOfMonthPayPeriod == null & obj.LastGivenDayOfWeekPay == null)
            {
                if (Budget.PaydayType != null)
                {
                    switch (Budget.PaydayType)
                    {
                        case "Everynth":
                            obj.Everynth = true;
                            obj.WorkingDays = false;
                            obj.OfEveryMonth = false;
                            obj.LastOfTheMonth = false;

                            obj.PeriodicPayPeriod = Budget.PaydayValue.GetValueOrDefault();
                            obj.PeriodicPayPeriodDDL = Budget.PaydayDuration;
                            break;
                        case "WorkingDays":
                            obj.Everynth = false;
                            obj.WorkingDays = true;
                            obj.OfEveryMonth = false;
                            obj.LastOfTheMonth = false;

                            obj.LastDayOfMonthPayPeriod = Budget.PaydayValue.GetValueOrDefault();
                            break;
                        case "OfEveryMonth":
                            obj.Everynth = false;
                            obj.WorkingDays = false;
                            obj.OfEveryMonth = true;
                            obj.LastOfTheMonth = false;

                            obj.GivenDayOfMonthPayPeriod = Budget.PaydayValue.GetValueOrDefault();
                            break;
                        case "LastOfTheMonth":
                            obj.Everynth = false;
                            obj.WorkingDays = false;
                            obj.OfEveryMonth = false;
                            obj.LastOfTheMonth = true;

                            obj.LastGivenDayOfWeekPay = Budget.PaydayDuration;
                            break;
                    }
                }
                else
                {
                    obj.Everynth = false;
                    obj.WorkingDays = false;
                    obj.OfEveryMonth = true;
                    obj.LastOfTheMonth = false;

                    obj.GivenDayOfMonthPayPeriod = 26;
                    obj.LastDayOfMonthPayPeriod = 3;
                    obj.PeriodicPayPeriod = 14;
                }
            }

            if (obj.CurrencyDDL == null & obj.CurrencyPlacementDDL == null & obj.DateFormatDDL == null & obj.NumberFormatDDL == null)
            {
                var BudgetSetting = _db.BudgetSettings.Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
                BudgetSettings? BS = BudgetSetting.FirstOrDefault();

                if (BS == null)
                {
                    obj.CurrencyDDL = "1";
                    obj.CurrencyPlacementDDL = "1";
                    obj.DateFormatDDL = "1";
                    obj.NumberFormatDDL = "1";
                }
                else
                {
                    obj.CurrencyDDL = BS.CurrencySymbol.ToString();
                    obj.CurrencyPlacementDDL = BS.CurrencyPattern.ToString();

                    var DateFormats = _db.lut_DateFormats.Where(x => x.DateSeperatorID == BS.DateSeperator && x.ShortDatePatternID == BS.ShortDatePattern);
                    lut_DateFormat DateFormat = DateFormats.FirstOrDefault();
                    obj.DateFormatDDL = DateFormat.id.ToString();

                    var NumberFormats = _db.lut_NumberFormats.Where(x => x.CurrencyDecimalDigitsID == BS.CurrencyDecimalDigits && x.CurrencyDecimalDigitsID == BS.CurrencyDecimalDigits && x.CurrencyGroupSeparatorID == BS.CurrencyGroupSeparator);
                    lut_NumberFormat NumberFormat = NumberFormats.FirstOrDefault();
                    obj.NumberFormatDDL = NumberFormat.id.ToString();
                }
            }

            TempData["PageHeading"] = "Time to create " + Budget?.BudgetName;

            ViewBag.CurrencyDDL = GetCurrencyDDL(obj.CurrencyDDL);
            ViewBag.NumberFormatDDL = GetNumberFormatDDL(obj.NumberFormatDDL);
            ViewBag.CurrencyPlacementDDL = GetCurrencyPlacementDDL(obj.CurrencyPlacementDDL);
            ViewBag.DateFormatDDL = GetDateFormatDDL(obj.DateFormatDDL);
            ViewBag.CurrentDate = (DateTime.Today).AddDays(1);
            ViewBag.Stage = Stage;

            return obj;
        }

        public List<SelectListItem> GetCurrencyDDL(string? SelectedValue)
        {
            //string fileName = "wwwroot/JSON/Common-Currency.json";
            //string jsonString = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
            //List<CurrencySelector> CurrencyDDL = JsonSerializer.Deserialize<List<CurrencySelector>>(jsonString)!;

            if (SelectedValue == null)
            {
                SelectedValue = "1";
            }

            List<lut_CurrencySymbol> CurrencyDDL = _db.lut_CurrencySymbols.ToList();

            List<SelectListItem> Currencylist = new List<SelectListItem>();

            foreach (lut_CurrencySymbol currency in CurrencyDDL)
            {
                SelectListItem item = new SelectListItem("[" + currency.CurrencySymbol + "] - " + currency.Code + ": " + currency.Name, currency.id.ToString());
                Currencylist.Add(item);
            }

            foreach (var item in Currencylist)
            {
                if (item.Value == SelectedValue)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }

            return Currencylist;
        }

        public List<SelectListItem> GetNumberFormatDDL(string? SelectedValue)
        {

            if (SelectedValue == null)
            {
                SelectedValue = "1";
            }

            List<lut_NumberFormat> NumberFormat = _db.lut_NumberFormats.ToList();
            List<SelectListItem> NumberFormatList = new List<SelectListItem>();
            foreach (lut_NumberFormat Format in NumberFormat)
            {
                SelectListItem item = new SelectListItem(Format.NumberFormat, Format.id.ToString());
                NumberFormatList.Add(item);
            }           

            foreach (var item in NumberFormatList)
            {
                if (item.Value == SelectedValue)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }

            return NumberFormatList;
        }

        public List<SelectListItem> GetCurrencyPlacementDDL(string? SelectedValue)
        {
            if (SelectedValue == null)
            {
                SelectedValue = "1";
            }

            List<lut_CurrencyPlacement> CurrencyPlacement = _db.lut_CurrencyPlacements.ToList();
            List<SelectListItem> CurrencyPlacementList = new List<SelectListItem>();
            foreach (lut_CurrencyPlacement Placement in CurrencyPlacement)
            {
                SelectListItem item = new SelectListItem(Placement.CurrencyPlacement, Placement.id.ToString());
                CurrencyPlacementList.Add(item);
            }

            foreach (var item in CurrencyPlacementList)
            {
                if (item.Value == SelectedValue)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }

            return CurrencyPlacementList;
        }

        public List<SelectListItem> GetDateFormatDDL(string? SelectedValue)
        {
            if (SelectedValue == null)
            {
                SelectedValue = "1";
            }

            List<lut_DateFormat> DateFormat = _db.lut_DateFormats.ToList();
            List<SelectListItem> DateFormatList = new List<SelectListItem>();
            foreach (lut_DateFormat Format in DateFormat)
            {
                SelectListItem item = new SelectListItem(Format.DateFormat, Format.id.ToString());
                DateFormatList.Add(item);
            }

            foreach (var item in DateFormatList)
            {
                if (item.Value == SelectedValue)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }

            return DateFormatList;
        }

    }
}
