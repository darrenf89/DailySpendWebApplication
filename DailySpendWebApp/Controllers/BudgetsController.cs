
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DailySpendBudgetWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DailySpendBudgetWebApp.Migrations;

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBudgetDetails(CreateABudgetPageModel obj)
        {
            if (ModelState.IsValid)
            {
                //ModelState.AddModelError("NextPayDayAmount", "");

                var Budgets = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));

                Budgets? Budget = Budgets.FirstOrDefault();

                Decimal Balance;
                Decimal.TryParse(obj.StartingBalance, out Balance);

                Budget.BankBalance = Balance;

                Decimal Pay;
                Decimal.TryParse(obj.NextPayDayAmount, out Pay);

                Budget.BankBalance = Balance;


                int year;
                int.TryParse(obj.NextIncomeDate.Split("-")[0], out year);
                int month;
                int.TryParse(obj.NextIncomeDate.Split("-")[1], out month);
                int day;
                int.TryParse(obj.NextIncomeDate.Split("-")[2], out day);

                Budget.NextIncomePayday = new DateTime(year, month, day);

                bool Everynth = obj.Everynth ?? false;
                bool WorkingDays = obj.WorkingDays ?? false;
                bool OfEveryMonth = obj.OfEveryMonth ?? false;
                bool LastOfTheMonth = obj.LastOfTheMonth ?? false;

                if (Everynth)
                {
                    Budget.PaydayType = "Everynth";
                    Budget.PaydayValue = obj.PeriodicPayPeriod;
                    Budget.PaydayDuration = obj.PeriodicPayPeriodDDL;
                }
                else if(WorkingDays)
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

            }

            obj = UpdateEnterBudgetDetailsPageFromModel(obj);
            ViewBag.Stage = "SaveBD";
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

                obj = UpdateEnterBudgetDetailsPageFromModel(obj);
                ViewBag.Stage = "Initial";
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
                    obj.StartingBalance = String.Format("{0:c}", Budget.BankBalance);
                }
                else
                {
                    obj.StartingBalance = null;
                }
            }

            if (obj.Everynth == null & obj.WorkingDays == null & obj.OfEveryMonth == null & obj.LastOfTheMonth == null)
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
                            obj.WorkingDays = false;
                            obj.OfEveryMonth = true;
                            obj.LastOfTheMonth = false;

                            obj.LastDayOfMonthPayPeriod = Budget.PaydayValue.GetValueOrDefault();
                            break;
                        case "OfEveryMonth":
                            obj.Everynth = false;
                            obj.WorkingDays = true;
                            obj.OfEveryMonth = false;
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
