
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DailySpendBudgetWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Data.SqlClient.Server;
using static System.Net.Mime.MediaTypeNames;
using DailySpendWebApp.Services;
using System.Linq;

namespace DailySpendBudgetWebApp.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDBContext _db;

        private readonly IProductTools _pt;

        private readonly IHttpContextAccessor _ca;


        public BudgetsController(ILogger<HomeController> logger, ApplicationDBContext db, IProductTools pt, IHttpContextAccessor ca)
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

        public void SaveSettingsAndDetails(CreateABudgetPageModel obj, Budgets? Budget, BudgetSettings? BS)
        {

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

            bool Everynth = obj.Everynth ?? false;
            bool WorkingDays = obj.WorkingDays ?? false;
            bool OfEveryMonth = obj.OfEveryMonth ?? false;
            bool LastOfTheMonth = obj.LastOfTheMonth ?? false;


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
            Budget.NextIncomePaydayCalculated = PayDate;

            if (Everynth)
            {
                Budget.PaydayType = "Everynth";
                Budget.PaydayValue = obj.PeriodicPayPeriod;
                Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                int Duration = new int();
                if (Budget.PaydayDuration == "days")
                {
                    Duration = 1;
                }
                else if (Budget.PaydayDuration == "weeks")
                {
                    Duration = 7;
                }
                else if (Budget.PaydayDuration == "years")
                {
                    Duration = 365;
                }
                Budget.AproxDaysBetweenPay = Duration * Budget.PaydayValue;
            }
            else if (WorkingDays)
            {
                Budget.PaydayType = "WorkingDays";
                Budget.PaydayValue = obj.LastDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int? NumberOfDaysBefore = Budget.PaydayValue;

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastWorkingDay(NumberOfDaysBefore);

            }
            else if (OfEveryMonth)
            {
                Budget.PaydayType = "OfEveryMonth";
                Budget.PaydayValue = obj.GivenDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int days = DateTime.DaysInMonth(year, month);
                Budget.AproxDaysBetweenPay = days;
            }
            else if (LastOfTheMonth)
            {
                Budget.PaydayType = "LastOfTheMonth";
                Budget.PaydayValue = null;
                Budget.PaydayDuration = obj.LastGivenDayOfWeekPay;

                int dayNumber = ((int)Enum.Parse(typeof(DayOfWeek), Budget.PaydayDuration));

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastDayOfWeek(dayNumber);

            }

            TimeSpan t = Budget.NextIncomePayday - DateTime.UtcNow ?? new TimeSpan();
            Budget.LeftToSpendDailyAmount = Balance / t.Days;

            _db.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/CreateBudget/{SaveYesNo?}")]
        public IActionResult CreateBudget(CreateABudgetPageModel obj, string SaveYesNo)
        {
            obj = ValidateBudgetDetails(obj);

            var BudgetsSetting = _db.BudgetSettings?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));

            BudgetSettings? BS = BudgetsSetting.FirstOrDefault();

            if (BS == null)
            {
                BS = new BudgetSettings();
                _db.BudgetSettings.Add(BS);
                BS.BudgetID = HttpContext.Session.GetInt32("_NewBudgetID");
            }

            var Budgets = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));

            Budgets? Budget = Budgets.FirstOrDefault();

            if (SaveYesNo == "Yes")
            {
                if (ModelState.IsValid)
                {
                    SaveSettingsAndDetails(obj, Budget, BS);
                }
            }
            else if (SaveYesNo == "No")
            {
                if ((Budget.BankBalance == null | BS.BudgetID == null))
                {
                    obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                    obj.Stage.Clear();

                    ViewBag.SnackBarMess = "SettingDetailsNotSaved";
                    ViewBag.SnackBarType = "danger";

                    return View("EnterBudgetDetails", obj);
                }
            }
            else
            {
                if (!(obj.Stage.Contains("SaveBD") & obj.Stage.Contains("SaveBS")))
                {
                    obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                    obj.Stage.Clear();
                    obj.Stage.Add("ReSaveDetailsSettings");
                    return View("EnterBudgetDetails", obj);
                }
            }

            string status = "";

            try
            {
                status = _pt.UpdateBudgetCreateSavings(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateIncome(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateSavingsSpend(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateBillsSpend(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);

                Budget.BudgetValuesLastUpdated = DateTime.UtcNow;
                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                Budget.LeftToSpendDailyAmount = (Budget.LeftToSpendBalance ?? 0) / DaysToPayDay;
                Budget.StartDayDailyAmount = Budget.LeftToSpendDailyAmount;

                //TODO: Create Buget Pay Period Stats
                status = _pt.CreateNewPayPeriodStats(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message, Status = status });

            }

            Budget.BudgetValuesLastUpdated = DateTime.UtcNow;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBudgetDetailsStage(CreateABudgetPageModel obj)
        {

            obj.Stage.Add("EditBD");
            obj.Stage.Remove("SaveBD");
            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBudgetSettingsStage(CreateABudgetPageModel obj)
        {
            obj.Stage.Add("EditBS");
            obj.Stage.Remove("SaveBS");
            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBudgetSettings(CreateABudgetPageModel obj)
        {

            if (ModelState.IsValid)
            {
                obj.Stage.Remove("EditBS");
                obj.Stage.Add("SaveBS");
                if (obj.Stage.Contains("SettingsAccordianOpen"))
                {
                    obj.Stage[obj.Stage.IndexOf("SettingsAccordianOpen")] = "SettingsAccordianClosed";
                }
                else
                {
                    obj.Stage.Add("SettingsAccordianClosed");
                }
                var BudgetsSetting = _db.BudgetSettings?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));

                BudgetSettings? BS = BudgetsSetting.FirstOrDefault();

                if (BS == null)
                {
                    BS = new BudgetSettings();
                    _db.BudgetSettings.Add(BS);
                    BS.BudgetID = HttpContext.Session.GetInt32("_NewBudgetID");
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
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            }

            ViewBag.SnackBarMess = "BudgetSettingsSaved";
            ViewBag.SnackBarType = "success";

            return View("EnterBudgetDetails", obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBudgetDetails(CreateABudgetPageModel obj)
        {
            obj = ValidateBudgetDetails(obj);

            if (ModelState.IsValid)
            {
                obj.Stage.Remove("EditBD");
                obj.Stage.Add("SaveBD");
                if (obj.Stage.Contains("DetailsAccordianOpen"))
                {
                    obj.Stage[obj.Stage.IndexOf("DetailsAccordianOpen")] = "DetailsAccordianClosed";
                }
                else
                {
                    obj.Stage.Add("DetailsAccordianClosed");
                }
                bool Everynth = obj.Everynth ?? false;
                bool WorkingDays = obj.WorkingDays ?? false;
                bool OfEveryMonth = obj.OfEveryMonth ?? false;
                bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

                var Budgets = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));

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
                Budget.NextIncomePaydayCalculated = PayDate;


                if (Everynth)
                {
                    Budget.PaydayType = "Everynth";
                    Budget.PaydayValue = obj.PeriodicPayPeriod;
                    Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                    int Duration = new int();
                    if (Budget.PaydayDuration == "days")
                    {
                        Duration = 1;
                    }
                    else if (Budget.PaydayDuration == "weeks")
                    {
                        Duration = 7;
                    }
                    else if (Budget.PaydayDuration == "years")
                    {
                        Duration = 365;
                    }
                    Budget.AproxDaysBetweenPay = Duration * Budget.PaydayValue;
                }
                else if (WorkingDays)
                {
                    Budget.PaydayType = "WorkingDays";
                    Budget.PaydayValue = obj.LastDayOfMonthPayPeriod;
                    Budget.PaydayDuration = null;

                    int? NumberOfDaysBefore = Budget.PaydayValue;

                    Budget.AproxDaysBetweenPay = GetNumberOfDaysLastWorkingDay(NumberOfDaysBefore);

                }
                else if (OfEveryMonth)
                {
                    Budget.PaydayType = "OfEveryMonth";
                    Budget.PaydayValue = obj.GivenDayOfMonthPayPeriod;
                    Budget.PaydayDuration = null;

                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;
                    int days = DateTime.DaysInMonth(year, month);
                    Budget.AproxDaysBetweenPay = days;
                }
                else if (LastOfTheMonth)
                {
                    Budget.PaydayType = "LastOfTheMonth";
                    Budget.PaydayValue = null;
                    Budget.PaydayDuration = obj.LastGivenDayOfWeekPay;

                    int dayNumber = ((int)Enum.Parse(typeof(DayOfWeek), Budget.PaydayDuration));

                    Budget.AproxDaysBetweenPay = GetNumberOfDaysLastDayOfWeek(dayNumber);

                }

                TimeSpan t = Budget.NextIncomePayday - DateTime.UtcNow ?? new TimeSpan();
                Budget.LeftToSpendDailyAmount = Balance / t.Days;

                _db.SaveChanges();
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            }

            ViewBag.SnackBarMess = "BudgetDetailsSaved";
            ViewBag.SnackBarType = "success";

            return View("EnterBudgetDetails", obj);
        }

        public int GetNumberOfDaysLastDayOfWeek(int dayNumber)
        {

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int NextYear = new int();
            int NextMonth = new int();

            if (month != 12)
            {
                NextYear = DateTime.Now.Year;
                NextMonth = month + 1;
            }
            else
            {
                NextYear = year + 1;
                NextMonth = 1;
            }

            DateTime CurrentDate = new DateTime();

            var i = DateTime.DaysInMonth(year, month);
            while (i > 0)
            {
                var dtCurrent = new DateTime(year, month, i);
                if ((int)dtCurrent.DayOfWeek == dayNumber)
                {
                    CurrentDate = dtCurrent;
                    i = 0;
                }
                else
                {
                    i = i - 1;
                }
            }


            DateTime NextDate = new DateTime();

            i = DateTime.DaysInMonth(NextYear, NextMonth);
            while (i > 0)
            {
                var dtCurrent = new DateTime(NextYear, NextMonth, i);
                if ((int)dtCurrent.DayOfWeek == dayNumber)
                {
                    NextDate = dtCurrent;
                    i = 0;
                }
                else
                {
                    i = i - 1;
                }
            }

            int DaysBetweenPay = (NextDate.Date - CurrentDate.Date).Days;

            return DaysBetweenPay;
        }

        public int GetNumberOfDaysLastWorkingDay(int? NumberOfDaysBefore)
        {
            if (NumberOfDaysBefore == null)
            {
                NumberOfDaysBefore = 1;
            }

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int NextYear = new int();
            int NextMonth = new int();

            if (month != 12)
            {
                NextYear = DateTime.Now.Year;
                NextMonth = month + 1;
            }
            else
            {
                NextYear = year + 1;
                NextMonth = 1;
            }

            DateTime CurrentDate = new DateTime();
            var i = DateTime.DaysInMonth(year, month);
            int j = 1;
            while (i > 0)
            {
                var dtCurrent = new DateTime(year, month, i);
                if (dtCurrent.DayOfWeek < DayOfWeek.Saturday && dtCurrent.DayOfWeek > DayOfWeek.Sunday)
                {
                    CurrentDate = dtCurrent;
                    if (j == NumberOfDaysBefore)
                    {
                        i = 0;
                    }
                    else
                    {
                        i = i - 1;
                        j = j + 1;
                    }
                }
                else
                {
                    i = i - 1;
                }
            }

            DateTime NextDate = new DateTime();
            i = DateTime.DaysInMonth(NextYear, NextMonth);
            j = 1;
            while (i > 0)
            {
                var dtCurrent = new DateTime(NextYear, NextMonth, i);
                if (dtCurrent.DayOfWeek < DayOfWeek.Saturday && dtCurrent.DayOfWeek > DayOfWeek.Sunday)
                {
                    NextDate = dtCurrent;
                    if (j == NumberOfDaysBefore)
                    {
                        i = 0;
                    }
                    else
                    {
                        i = i - 1;
                        j = j + 1;
                    }
                }
                else
                {
                    i = i - 1;
                }
            }

            int DaysBetweenPay = (NextDate.Date - CurrentDate.Date).Days;

            return DaysBetweenPay;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/DeleteIncome/{id?}")]
        public IActionResult DeleteIncome(CreateABudgetPageModel obj, int id)
        {

            var IncomeSet = _db.IncomeEvents.Where(x => x.IncomeEventID == id);
            var Income = IncomeSet.FirstOrDefault();
            _db.IncomeEvents.Remove(Income);
            _db.SaveChanges();

            obj.IncomeName = Income.IncomeName;
            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            ViewBag.SnackBarMess = "DeleteIncome";
            ViewBag.SnackBarType = "danger";

            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/EditIncome/{id?}")]
        public IActionResult EditIncome(CreateABudgetPageModel obj, int id)
        {

            var IncomeSet = _db.IncomeEvents.Where(x => x.IncomeEventID == id);
            var Income = IncomeSet.FirstOrDefault();

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            obj.IncomeName = Income.IncomeName;
            obj.IncomeEventID = Income.IncomeEventID;

            obj.Stage.Add("EditIncome");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/DeleteBill/{id?}")]
        public IActionResult DeleteBill(CreateABudgetPageModel obj, int id)
        {

            var BillSet = _db.Bills.Where(x => x.BillID == id);
            var Bill = BillSet.FirstOrDefault();
            _db.Bills.Remove(Bill);
            _db.SaveChanges();

            obj.BillName = Bill.BillName;
            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            ViewBag.SnackBarMess = "DeleteBill";
            ViewBag.SnackBarType = "danger";

            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/EditBill/{id?}")]
        public IActionResult EditBill(CreateABudgetPageModel obj, int id)
        {

            var BillSet = _db.Bills.Where(x => x.BillID == id);
            var Bill = BillSet.FirstOrDefault();

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            obj.BillID = Bill.BillID;

            obj.Stage.Add("EditBill");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/DeleteSaving/{id?}")]
        public IActionResult DeleteSaving(CreateABudgetPageModel obj, int id)
        {

            var SavingSet = _db.Savings.Where(x => x.SavingID == id);
            var Saving = SavingSet.FirstOrDefault();
            _db.Savings.Remove(Saving);
            _db.SaveChanges();

            obj.SavingsName = Saving.SavingsName;
            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            ViewBag.SnackBarMess = "DeleteSaving";
            ViewBag.SnackBarType = "danger";

            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Budgets/EditSaving/{id?}")]
        public IActionResult EditSaving(CreateABudgetPageModel obj, int id)
        {

            var SavingSet = _db.Savings.Where(x => x.SavingID == id);
            var Saving = SavingSet.FirstOrDefault();

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            obj.SavingID = Saving.SavingID;

            obj.Stage.Add("EditSaving");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIncomeBudget(CreateABudgetPageModel obj)
        {
            IncomeEvents? I = new();

            var BudgetSavingsList = _db.Budgets?
               .Include(x => x.IncomeEvents)
               .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
               .ToList();

            if (obj.isInstantActive == null)
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

            if (!obj.Stage.Contains("EditIncome"))
            {
                var BudgetBillssList = _db.Budgets?
                    .Include(x => x.IncomeEvents)
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                    .ToList();

                foreach (var income in BudgetBillssList[0].IncomeEvents)
                {
                    if (income.IncomeName == obj.IncomeName && !income.isClosed)
                    {
                        ModelState.AddModelError("IncomeName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                        break;
                    }
                }
            }
            else
            {
                var IncomesList = _db.IncomeEvents.Where(x => x.IncomeEventID == obj.IncomeEventID);
                I = IncomesList.FirstOrDefault();
            }

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            if (ModelState.IsValid)
            {
                I.IncomeAmount = obj.IncomeAmount ?? 0;
                I.DateOfIncomeEvent = obj.DateOfIncomeEvent ?? DateTime.UtcNow;
                I.IncomeName = obj.IncomeName;
                I.isClosed = false;
                I.isRecurringIncome = obj.isRecurringIncome ?? false;
                I.isInstantActive = obj.isInstantActive;

                if (obj.isInstantActive ?? false)
                {
                    I.IncomeActiveDate = obj.IncomeActiveDate ?? DateTime.UtcNow;
                }
                else
                {
                    I.IncomeActiveDate = obj.DateOfIncomeEvent ?? DateTime.UtcNow;
                }

                if (obj.isRecurringIncome ?? false)
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

                if (!obj.Stage.Contains("EditIncome"))
                {
                    _db.Attach(Budget);
                    Budget.IncomeEvents.Add(I);
                    ViewBag.SnackBarMess = "NewIncomeAdded";
                    ViewBag.SnackBarType = "success";
                }
                else
                {
                    ViewBag.SnackBarMess = "IncomeEdited";
                    ViewBag.SnackBarType = "success";
                }

                _db.SaveChanges();

            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                obj.Stage.Remove("EditIncome");
                return View("EnterBudgetDetails", obj);
            }

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            obj.Stage.Remove("EditIncome");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBillBudget(CreateABudgetPageModel obj)
        {
            Bills? B = new();

            if (obj.BillName == null | obj.BillName == "")
            {
                ModelState.AddModelError("SavingsName", "* You have to enter a name for your saving.");
            }

            if ((obj.BillValue < 1 | obj.BillValue > 31) && obj.BillType == "OfEveryMonth")
            {
                ModelState.AddModelError("BillName", "* You have entered an invalid value for your bill. you must enter a day between between 1 and 31");
            }

            if (!obj.Stage.Contains("EditBill"))
            {
                var BudgetBillssList = _db.Budgets?
                    .Include(x => x.Bills)
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                    .ToList();

                foreach (var bill in BudgetBillssList[0].Bills)
                {
                    if (bill.BillName == obj.BillName && !bill.isClosed)
                    {
                        ModelState.AddModelError("BillName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                        break;
                    }
                }
            }
            else
            {
                var BillsList = _db.Bills.Where(x => x.BillID == obj.BillID);
                B = BillsList.FirstOrDefault();
            }

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            if (ModelState.IsValid)
            {
                decimal DailySavingValue = new();
                TimeSpan Difference = (TimeSpan)(obj.BillDueDate - DateTime.Now);
                int NumberOfDays = Difference.Days;
                decimal BillAmount = obj.BillAmount ?? 0;
                decimal BillCurrentBalance = obj.BillCurrentBalance ?? 0;
                decimal RemainingBillAmount = BillAmount - BillCurrentBalance;
                DailySavingValue = RemainingBillAmount / NumberOfDays;
                DailySavingValue = Math.Round(DailySavingValue, 2);

                B.RegularBillValue = DailySavingValue;
                B.BillName = obj.BillName;
                B.isRecuring = obj.isBillRecuring ?? false;
                B.BillDueDate = obj.BillDueDate;
                B.BillCurrentBalance = BillCurrentBalance;
                B.LastUpdatedDate = DateTime.UtcNow;
                B.BillAmount = obj.BillAmount;

                if (obj.isBillRecuring ?? false)
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

                if (!obj.Stage.Contains("EditBill"))
                {
                    _db.Attach(Budget);
                    Budget.Bills.Add(B);
                    ViewBag.SnackBarMess = "NewBillAdded";
                    ViewBag.SnackBarType = "success";
                }
                else
                {
                    ViewBag.SnackBarMess = "BillEdited";
                    ViewBag.SnackBarType = "success";
                }

                _db.SaveChanges();

            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                obj.Stage.Remove("EditBill");
                return View("EnterBudgetDetails", obj);
            }

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            obj.Stage.Remove("EditBill");
            return View("EnterBudgetDetails", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSavingBudget(CreateABudgetPageModel obj)
        {

            Savings? S = new();

            if (obj.SavingsName == null | obj.SavingsName == "")
            {
                ModelState.AddModelError("SavingsName", "* You have to enter a name for your saving.");
            }

            if (!obj.Stage.Contains("EditSaving"))
            {

                var BudgetSavingsList = _db.Budgets?
                    .Include(x => x.Savings)
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                    .ToList();

                foreach (var saving in BudgetSavingsList[0].Savings)
                {
                    if (saving.SavingsName == obj.SavingsName && !saving.isSavingsClosed)
                    {
                        ModelState.AddModelError("SavingsName", "* You have already used that Name. Dont save for the same thing twice, just do it once.");
                        break;
                    }
                }
            }
            else
            {
                var SavingsList = _db.Savings.Where(x => x.SavingID == obj.SavingID);
                S = SavingsList.FirstOrDefault();
            }

            if (DateTime.UtcNow.AddDays(1) >= obj.GoalDate)
            {
                ModelState.AddModelError("GoalDate", "* Please add a date in the future .. you know your next pay day!");
            }



            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
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
                        S.RegularSavingValue = obj.RegularSavingValue;
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

                if (!obj.Stage.Contains("EditSaving"))
                {
                    _db.Attach(Budget);
                    Budget.Savings.Add(S);
                    ViewBag.SnackBarMess = "NewSavingAdded";
                    ViewBag.SnackBarType = "success";
                }
                else
                {
                    ViewBag.SnackBarMess = "SavingEdited";
                    ViewBag.SnackBarType = "success";
                }

                _db.SaveChanges();

            }
            else
            {
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                obj.Stage.Remove("EditSaving");
                return View("EnterBudgetDetails", obj);
            }


            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            obj.Stage.Remove("EditSaving");
            return View("EnterBudgetDetails", obj);
        }

        [Route("Budgets/AddSavingPV/{type?}/{id?}")]
        public IActionResult AddSavingPV(CreateABudgetPageModel obj, string? type, int? id)
        {

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            if (type == "Edit")
            {
                var SavingSet = _db.Savings.Where(x => x.SavingID == id);
                var Saving = SavingSet.FirstOrDefault();

                obj.SavingID = Saving.SavingID;
                obj.isRegularSaving = Saving.isRegularSaving;
                obj.SavingsType = Saving.SavingsType;
                obj.SavingsName = Saving.SavingsName;
                obj.CurrentBalance = Saving.CurrentBalance;
                obj.RegularSavingValue = Saving.RegularSavingValue;
                obj.canExceedGoal = Saving.canExceedGoal;
                obj.GoalDate = Saving.GoalDate;
                obj.isAutoComplete = Saving.isAutoComplete;
                obj.isDailySaving = Saving.isDailySaving;
                obj.SavingsGoal = Saving.SavingsGoal;

                obj.Stage.Add("EditSaving");
            }
            else if (type == "New")
            {
                obj.Stage.Remove("EditSaving");
            }


            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            return PartialView("_SavingsModalPV", obj);
        }

        [Route("Budgets/AddIncomePV/{type?}/{id?}")]
        public IActionResult AddIncomePV(CreateABudgetPageModel obj, string? type, int? id)
        {

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            if (type == "Edit")
            {
                var IncomeSet = _db.IncomeEvents.Where(x => x.IncomeEventID == id);
                var Income = IncomeSet.FirstOrDefault();

                obj.IncomeEventID = Income.IncomeEventID;
                obj.IncomeAmount = Income.IncomeAmount;
                obj.IncomeName = Income.IncomeName;
                obj.IncomeActiveDate = Income.IncomeActiveDate;
                obj.DateOfIncomeEvent = Income.DateOfIncomeEvent;
                obj.isRecurringIncome = Income.isRecurringIncome;
                obj.RecurringIncomeType = Income.RecurringIncomeType;
                obj.RecurringIncomeValue = Income.RecurringIncomeValue;
                obj.RecurringIncomeDuration = Income.RecurringIncomeDuration;
                obj.isClosed = Income.isClosed;
                obj.isInstantActive = Income.isInstantActive;

                obj.Stage.Add("EditIncome");
            }
            else if (type == "New")
            {
                obj.Stage.Remove("EditIncome");
            }


            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            return PartialView("_IncomeModalPV", obj);
        }

        [Route("Budgets/AddBillPV/{type?}/{id?}")]
        public IActionResult AddBillPV(CreateABudgetPageModel obj, string? type, int? id)
        {

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);

            if (type == "Edit")
            {
                var BillSet = _db.Bills.Where(x => x.BillID == id);
                var Bill = BillSet.FirstOrDefault();

                obj.BillID = Bill.BillID;
                obj.RegularBillValue = Bill.RegularBillValue;
                obj.BillName = Bill.BillName;
                obj.BillType = Bill.BillType;
                obj.BillValue = Bill.BillValue;
                obj.BillDuration = Bill.BillDuration;
                obj.BillAmount = Bill.BillAmount;
                obj.BillDueDate = Bill.BillDueDate;
                obj.isBillRecuring = Bill.isRecuring;

                obj.Stage.Add("EditBill");
            }
            else if (type == "New")
            {
                obj.Stage.Remove("EditBill");
            }


            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            return PartialView("_BillsModalPV", obj);
        }

        public IActionResult CreateNewBudget()
        {
            return View();
        }

        public IActionResult EnterBudgetDetails(CreateABudgetPageModel obj)
        {
            if (HttpContext.Session.GetInt32("_NewBudgetID") != null)
            {
                obj.Stage.Add("Initial");
                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
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

        public CreateABudgetPageModel UpdateEnterBudgetDetailsPageFromModel(CreateABudgetPageModel obj)
        {

            var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
            Budgets Budget = BudgetList.FirstOrDefault();

            ViewBag.PaymentPeriod = Budget?.AproxDaysBetweenPay;
            ViewBag.hasBudgetName = true;
            ViewBag.BudgetName = Budget?.BudgetName;

            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.CurrencySymbol = "£";
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = ",";
            nfi.CurrencyDecimalDigits = 2;
            nfi.CurrencyPositivePattern = 0;

            if (Budget.AproxDaysBetweenPay != null)
            {
                obj.AproxDaysBetweenPay = Budget.AproxDaysBetweenPay;
            }

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

                    obj.StartingBalance = String.Format(nfi, "{0:c}", Budget.BankBalance);
                }
                else
                {
                    obj.StartingBalance = null;
                }
            }
            else
            {
                string PayAmountString = String.Format(nfi, "{0:c}", decimal.Parse(obj.StartingBalance));
                obj.StartingBalance = PayAmountString;
            }

            if (obj.NextPayDayAmount == null)
            {
                if (Budget.PaydayAmount != null & Budget.PaydayAmount != 0)
                {
                    obj.NextPayDayAmount = String.Format(nfi, "{0:c}", Budget.PaydayAmount);
                }
                else
                {
                    obj.NextPayDayAmount = null;
                }
            }
            else
            {
                string PayAmountString = String.Format(nfi, "{0:c}", decimal.Parse(obj.NextPayDayAmount));
                obj.NextPayDayAmount = PayAmountString;
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

            var temp = new List<string?>();
            foreach (var s in obj.Stage)
            {
                if (!string.IsNullOrEmpty(s))
                    temp.Add(s);
            }
            obj.Stage = temp;

            return obj;
        }

        public IActionResult AddSavingsTable()
        {
            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            Budgets? BudgetSavingsList = _db.Budgets?
                .Include(x => x.Savings)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                .FirstOrDefault();

            List<Savings> SavingsList = new List<Savings>();
            foreach (Savings saving in BudgetSavingsList.Savings)
            {
                SavingsList.Add(saving);
            }
            return PartialView("SavingsTableBudget", SavingsList);
        }

        public IActionResult AddIncomesTable()
        {

            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            Budgets? BudgetIncomeEventList = _db.Budgets?
                .Include(x => x.IncomeEvents)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                .FirstOrDefault();

            List<IncomeEvents> IncomeEventsList = new List<IncomeEvents>();
            foreach (IncomeEvents IncomeEvents in BudgetIncomeEventList.IncomeEvents)
            {
                IncomeEventsList.Add(IncomeEvents);
            }
            return PartialView("IncomeTableBudget", IncomeEventsList);
        }

        public IActionResult AddBillsTable()
        {
            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;

            Budgets? BudgetBillsList = _db.Budgets?
                .Include(x => x.Bills)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"))
                .FirstOrDefault();

            List<Bills> BillsList = new List<Bills>();
            foreach (Bills bill in BudgetBillsList.Bills)
            {
                BillsList.Add(bill);
            }
            return PartialView("BillsTableBudget", BillsList);
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

        public IActionResult EditPayInfo(CreateABudgetPageModel obj)
        {
            var BudgetList = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

            Budgets Budget = BudgetList.FirstOrDefault();

            obj.BankBalance = Budget.BankBalance;
            obj.LeftToSpendBalance = Budget.LeftToSpendBalance;
            obj.MoneyAvailableBalance = Budget.MoneyAvailableBalance;
            obj.NextPayDayAmount = string.Format("{0:c}", Budget.PaydayAmount);
            obj.NextIncomePayday = Budget.NextIncomePayday;

            ViewBag.CurrentDate = (DateTime.Today).AddDays(1);
            ViewBag.PaymentPeriod = Budget?.AproxDaysBetweenPay;
            AddUserCurrencyTimeViewBagPreferences();

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

            TempData["PageHeading"] = "Edit your Budget!";
            return View(obj);
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
            ViewBag.ShortDatePattern = _pt.GetBudgetShortDatePattern(HttpContext.Session.GetInt32("_DefaultBudgetID") ?? 0);
            ViewBag.DateSeperator = CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.DateSeparator;
        }

        [HttpPost]
        public IActionResult UpdateBudgetDetails(CreateABudgetPageModel obj)
        {
            var BudgetList = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

            Budgets Budget = BudgetList.FirstOrDefault();

            _db.Attach(Budget);

            bool Everynth = obj.Everynth ?? false;
            bool WorkingDays = obj.WorkingDays ?? false;
            bool OfEveryMonth = obj.OfEveryMonth ?? false;
            bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

            Decimal Pay;
            Decimal.TryParse(obj.NextPayDayAmount, out Pay);

            Budget.PaydayAmount = Pay;

            Budget.NextIncomePayday = obj.NextIncomePayday;
            Budget.NextIncomePaydayCalculated = obj.NextIncomePayday;


            if (Everynth)
            {
                Budget.PaydayType = "Everynth";
                Budget.PaydayValue = obj.PeriodicPayPeriod;
                Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                int Duration = new int();
                if (Budget.PaydayDuration == "days")
                {
                    Duration = 1;
                }
                else if (Budget.PaydayDuration == "weeks")
                {
                    Duration = 7;
                }
                else if (Budget.PaydayDuration == "years")
                {
                    Duration = 365;
                }
                Budget.AproxDaysBetweenPay = Duration * Budget.PaydayValue;
            }
            else if (WorkingDays)
            {
                Budget.PaydayType = "WorkingDays";
                Budget.PaydayValue = obj.LastDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int? NumberOfDaysBefore = Budget.PaydayValue;

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastWorkingDay(NumberOfDaysBefore);

            }
            else if (OfEveryMonth)
            {
                Budget.PaydayType = "OfEveryMonth";
                Budget.PaydayValue = obj.GivenDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int days = DateTime.DaysInMonth(year, month);
                Budget.AproxDaysBetweenPay = days;
            }
            else if (LastOfTheMonth)
            {
                Budget.PaydayType = "LastOfTheMonth";
                Budget.PaydayValue = null;
                Budget.PaydayDuration = obj.LastGivenDayOfWeekPay;

                int dayNumber = ((int)Enum.Parse(typeof(DayOfWeek), Budget.PaydayDuration));

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastDayOfWeek(dayNumber);
            }

            _db.SaveChanges();

            return RedirectToAction( "Index", "Home");
        }

        [HttpPost]
        public IActionResult RecalculateBudget(CreateABudgetPageModel obj)
        {
            var BudgetList = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

            Budgets Budget = BudgetList.FirstOrDefault();

            _db.Attach(Budget);

            bool Everynth = obj.Everynth ?? false;
            bool WorkingDays = obj.WorkingDays ?? false;
            bool OfEveryMonth = obj.OfEveryMonth ?? false;
            bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

            Decimal Pay;
            Decimal.TryParse(obj.NextPayDayAmount, out Pay);

            Budget.PaydayAmount = Pay;

            Budget.NextIncomePayday = obj.NextIncomePayday;
            Budget.NextIncomePaydayCalculated = obj.NextIncomePayday;


            if (Everynth)
            {
                Budget.PaydayType = "Everynth";
                Budget.PaydayValue = obj.PeriodicPayPeriod;
                Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                int Duration = new int();
                if (Budget.PaydayDuration == "days")
                {
                    Duration = 1;
                }
                else if (Budget.PaydayDuration == "weeks")
                {
                    Duration = 7;
                }
                else if (Budget.PaydayDuration == "years")
                {
                    Duration = 365;
                }
                Budget.AproxDaysBetweenPay = Duration * Budget.PaydayValue;
            }
            else if (WorkingDays)
            {
                Budget.PaydayType = "WorkingDays";
                Budget.PaydayValue = obj.LastDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int? NumberOfDaysBefore = Budget.PaydayValue;

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastWorkingDay(NumberOfDaysBefore);

            }
            else if (OfEveryMonth)
            {
                Budget.PaydayType = "OfEveryMonth";
                Budget.PaydayValue = obj.GivenDayOfMonthPayPeriod;
                Budget.PaydayDuration = null;

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int days = DateTime.DaysInMonth(year, month);
                Budget.AproxDaysBetweenPay = days;
            }
            else if (LastOfTheMonth)
            {
                Budget.PaydayType = "LastOfTheMonth";
                Budget.PaydayValue = null;
                Budget.PaydayDuration = obj.LastGivenDayOfWeekPay;

                int dayNumber = ((int)Enum.Parse(typeof(DayOfWeek), Budget.PaydayDuration));

                Budget.AproxDaysBetweenPay = GetNumberOfDaysLastDayOfWeek(dayNumber);
            }

            string status = "";

            _db.SaveChanges();

            try
            {
                status = _pt.UpdateBudgetCreateSavings(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateIncome(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateSavingsSpend(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);
                status = _pt.UpdateBudgetCreateBillsSpend(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);

                Budget.BudgetValuesLastUpdated = DateTime.UtcNow;
                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                Budget.LeftToSpendDailyAmount = (Budget.LeftToSpendBalance ?? 0) / DaysToPayDay;
                Budget.StartDayDailyAmount = Budget.LeftToSpendDailyAmount;

                //TODO: Update PayPeriod Stats.
                status = _pt.UpdatePayPeriodStats(HttpContext.Session.GetInt32("_NewBudgetID") ?? 0);

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { Message = ex.Message, Status = status });

            }

            

            return RedirectToAction("Index", "Home");
        }

    }
}
