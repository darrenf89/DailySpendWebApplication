using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace DailySpendBudgetWebApp.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly ApplicationDBContext _db;

        public BudgetsController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult AddSavingPV()
        {
            TempData["NickName"] = HttpContext.Session.GetString("_Name");
            TempData["PageHeading"] = "Add A New Saving!";

            var BudgetList = _db.Budgets?
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
            Budgets? Budget = BudgetList?.FirstOrDefault();

            ViewBag.PaymentPeriod = Budget.AproxDaysBetweenPay;
            ViewBag.CurrentDate = (DateTime.Today).AddDays(1);

            return PartialView("_SavingsModalPV");
        }

        public IActionResult CreateFirstBudget() 
        {
            return View();
        }

        public IActionResult EnterBudgetDetails()
        {

            if (HttpContext.Session.GetInt32("_NewBudgetID") != null)
            {
                TempData["PageHeading"] = "Time to create " + HttpContext.Session.GetString("_NewBudgetName");
                ViewBag.hasBudgetName = true;
                ViewBag.BudgetName = "Name";
            }
            else
            {
                TempData["PageHeading"] = "Create your Budget!";
                ViewBag.hasBudgetName = false;
            }

            List<CurrencySelector> DDL = GetCurrencyDDL();
            List<SelectListItem> Currencylist = new List<SelectListItem>();

            foreach (CurrencySelector currency in DDL) 
            {                 
                SelectListItem item = new SelectListItem(currency.code + " - " + currency.name, currency.symbol);
                Currencylist.Add(item);
            }

            ViewBag.CurrencyDDL = Currencylist;
            ViewBag.NumberFormatDDL = GetNumberFormatDDL();
            ViewBag.CurrencyPlacementDDL = GetCurrencyPlacementDDL();
            ViewBag.DateFormatDDL = GetDateFormatDDL();

            return View();
        }

        public IActionResult EditBudgetDetails()
        {
            if (HttpContext.Session.GetInt32("_DefaultBudgetID") != null)
            {
                //Get the DefaultBudgetID and the Budget Setting and prefill
                TempData["NickName"] = HttpContext.Session.GetString("_Name");
                TempData["PageHeading"] = "Update your Budget!";
                ViewBag.hasBudgetName = true;
                var BudgetList = _db.Budgets
                    .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"));
                Budgets Budget = BudgetList.FirstOrDefault();
                ViewBag.BudgetName = Budget.BudgetName;
            }
            else
            {
                TempData["NickName"] = HttpContext.Session.GetString("_Name");
                TempData["PageHeading"] = "Create your Budget!";
                ViewBag.hasBudgetName = false;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFirstBudget(CreateABudgetPageModel obj)
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

                HttpContext.Session.SetInt32("_NewBudgetID",B.BudgetID);
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

        public List<SelectListItem> GetCurrencyPlacementDDL()
        {
            List<SelectListItem> NumberFormatDDL = new List<SelectListItem>()
            {
                new SelectListItem {
                Text = "$n", Value = "1"
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
