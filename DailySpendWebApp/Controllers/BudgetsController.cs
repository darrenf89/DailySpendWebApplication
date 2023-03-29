
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using DailySpendBudgetWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
            return View();
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
                var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
                Budgets Budget = BudgetList.FirstOrDefault();

                ViewBag.PaymentPeriod = Budget?.AproxDaysBetweenPay;
                ViewBag.hasBudgetName = true;
                ViewBag.BudgetName = Budget?.BudgetName;

                if (Budget.NextIncomePayday != null)
                {
                    obj.NextIncomeDate = DateOnly.FromDateTime((DateTime)Budget.NextIncomePayday);
                }
                else
                {
                    obj.NextIncomeDate = null;
                }

                if (Budget.BankBalance != null | Budget.BankBalance != 0)
                {
                    obj.StartingBalance = String.Format("{0:c}", Budget.BankBalance);
                }
                else
                {
                    obj.StartingBalance = null;
                }

                var BudgetSetting = _db.BudgetSettings.Where(x => x.BudgetID == HttpContext.Session.GetInt32("_NewBudgetID"));
                BudgetSettings? BS = BudgetSetting.FirstOrDefault();

                if (BS == null)
                {
                    
                }
                else
                {
                    obj.CurrencyDDL = BS.CurrencySymbol.ToString();
                    obj.CurrencyPlacementDDL = BS.CurrencyPattern.ToString();
                }

                TempData["PageHeading"] = "Time to create " + Budget?.BudgetName;

                List<CurrencySelector> DDL = GetCurrencyDDL();
                List<SelectListItem> Currencylist = new List<SelectListItem>();
                
                var i = 1;
                foreach (CurrencySelector currency in DDL)
                {
                    string value = i.ToString();
                    SelectListItem item = new SelectListItem( currency.symbol+ ": " + currency.code + " - " + currency.name, value);
                    Currencylist.Add(item);
                    i += 1;
                }

                ViewBag.CurrencyDDL = Currencylist;
                ViewBag.NumberFormatDDL = GetNumberFormatDDL();
                ViewBag.CurrencyPlacementDDL = GetCurrencyPlacementDDL(obj.CurrencyPlacementDDL);
                ViewBag.DateFormatDDL = GetDateFormatDDL();
                ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

                return View();
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

        public List<CurrencySelector> GetCurrencyDDL()
        {
            string fileName = "wwwroot/JSON/Common-Currency.json";
            string jsonString = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
            List<CurrencySelector> CurrencyDDL = JsonSerializer.Deserialize<List<CurrencySelector>>(jsonString)!;

            return CurrencyDDL;
        }

        public List<SelectListItem> GetNumberFormatDDL()
        {
            List<SelectListItem> NumberFormatDDL = new List<SelectListItem>()
        {
            new SelectListItem {
            Text = "123.456,78", Value = "1"
            },
            new SelectListItem {
            Text = "123,456.78", Value = "2"
            },
            new SelectListItem {
            Text = "123.456", Value = "3"
            },
            new SelectListItem {
            Text = "123,456", Value = "4"
            },
            new SelectListItem {
            Text = "123 456-78", Value = "5"
            },
            new SelectListItem {
            Text = "123 456/78", Value = "6"
            },
            new SelectListItem {
            Text = "123 456/78", Value = "7"
            },
            new SelectListItem {
            Text = "123 456", Value = "8"
            },
        };


            return NumberFormatDDL;
        }

        public List<SelectListItem> GetCurrencyPlacementDDL(string? SelectedValue)
        {
            if (SelectedValue == null)
            {
                SelectedValue = "1";
            }

            List<SelectListItem> NumberFormatDDL = new List<SelectListItem>()
            {
                new SelectListItem {
                Text = "$n", Value = "1",
                },
                new SelectListItem {
                Text = "n$", Value = "2"
                },
                new SelectListItem {
                Text = "$ n", Value = "3"
                },
                new SelectListItem {
                Text = "n $", Value = "4"
                },
            };

            foreach (var item in NumberFormatDDL)
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

            return NumberFormatDDL;
        }

        public List<SelectListItem> GetDateFormatDDL()
        {
            List<SelectListItem> NumberFormatDDL = new List<SelectListItem>()
        {
            new SelectListItem {
            Text = "yyyy/mm/dd", Value = "1"
            },
            new SelectListItem {
            Text = "yyyy-mm-dd", Value = "2"
            },
            new SelectListItem {
            Text = "dd/mm/yyyy", Value = "3"
            },
            new SelectListItem {
            Text = "dd-mm-yyyy", Value = "4"
            },
            new SelectListItem {
            Text = "mm/dd/yyyy", Value = "5"
            },
            new SelectListItem {
            Text = "mm-dd-yyyy", Value = "6"
            },
        };


            return NumberFormatDDL;
        }

        //public List<SelectListItem> GetCurrencyDDL()
        //{
        //    List<SelectListItem> CurrencyDDL = new List<SelectListItem>()
        //    {
        //        new SelectListItem {
        //            Text = "C#", Value = "9"
        //        },
        //        new SelectListItem {
        //            Text = "AED - UAE dirham", Value = "\u062f"
        //        },
        //    };

        //    return CurrencyDDL;
        //}
    }
}
