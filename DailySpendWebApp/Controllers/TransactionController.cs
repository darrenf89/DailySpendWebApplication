
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
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;

namespace DailySpendWebApp.Controllers
{

    public class TransactionController : Controller
    {

        private readonly ILogger<TransactionController> _logger;

        private readonly ApplicationDBContext _db;

        private readonly IProductTools _pt;

        public TransactionController(ILogger<TransactionController> logger, ApplicationDBContext db, IProductTools pt)
        {
            _logger = logger;
            _db = db;
            _pt = pt;

            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = "£";
            nfi.NumberFormat.CurrencyDecimalSeparator = ".";
            nfi.NumberFormat.CurrencyGroupSeparator = ",";
            nfi.NumberFormat.CurrencyDecimalDigits = 2;
            nfi.NumberFormat.CurrencyPositivePattern = 0;

            Thread.CurrentThread.CurrentCulture = nfi;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTransactionSavingsAdd(Transactions obj)
        {

            Budgets? Budget = _db.Budgets?
                .Include(x => x.Transactions)
                .Include(x => x.Savings)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            _db.Attach(Budget);

            Transaction T = 

            string status = _pt.TransactSavingsTransaction(ref T, HttpContext.Session.GetInt32("_DefaultBudgetID"));
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTransactionSavingsSubtract(Transactions obj)
        {
            
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTransaction(Transactions obj)
        {
            if(obj.TransactionAmount == null | obj.TransactionAmount == 0)
            {
                ModelState.AddModelError("TransactionID", "* You cant add a Zero value transaction ... I assume you meant to spend some money?!");
            }
            else if(obj.TransactionDate == null | obj.TransactionDate.ToString() == "")
            {
                ModelState.AddModelError("TransactionID", "* You have to enter a date for the transaction. Dont worry it can be today, yesterday or tomoorow ... or any day just enter one!");
            }

            if (!ModelState.IsValid)
            {
                return View("AddTransaction", obj);
            }

            Budgets? Budget = _db.Budgets?
                .Include(x => x.Transactions)
                .Include(x => x.Savings)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            _db.Attach(Budget);

            Transactions T = new Transactions();
            Budget.Transactions.Add(T);

            T = obj;

            if (T.isIncome)
            {
                T.Category = null;
                T.CategoryID = 0;
            }

            if(T.isSpendFromSavings)
            {
                Savings S = Budget.Savings.Where(s => s.SavingID == T.SavingID).First();
                ViewBag.SavingsGoal = String.Format("{0:c}", S.SavingsGoal);
                ViewBag.CurrentBalance = String.Format("{0:c}", S.CurrentBalance);
                if (T.isIncome)
                {
                    if(S.SavingsGoal != null | S.SavingsGoal != 0)
                    {
                        ViewBag.Stage = "ConfirmSavingSpendIncome";
                    }
                    
                }
                else
                {
                    if (S.SavingsGoal != null | S.SavingsGoal != 0)
                    {
                        ViewBag.Stage = "ConfirmSavingSpendOutcome";
                    }
                }
                
                return View("AddTransaction", obj);
            }
            else
            {
                T.SavingID = 0;
                T.SavingName = null;
            }

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly TransactionDate = DateOnly.FromDateTime(T.TransactionDate ?? DateTime.Now);

            if (TransactionDate <= currentDate)
            {
                string status = _pt.TransactTransaction(ref T, HttpContext.Session.GetInt32("_DefaultBudgetID"));
                if (status != "OK")
                {
                    ModelState.AddModelError("TransactionID", "* There was a problem transacting the ... Transaction. Please try again!");
                    return View("AddTransaction", obj);
                }
            }

            _db.SaveChanges();

            TempData["PageHeading"] = "Add a Transaction!";
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AddTransaction(Transactions obj)
        {           

            TempData["PageHeading"] = "Add a Transaction!";
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectPayee(Transactions obj)
        {
            Budgets? Budget = _db.Budgets?
                .Include(x => x.Transactions)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            var Payee = Budget.Transactions.Select(t => t.Payee).Distinct().ToList();

            Payee.Sort();
            if (Payee.Count == 0) 
            {
                Payee.Add("No Payees");    
            }

            obj.PayeeList = Payee;
            TempData["PageHeading"] = "Select a Payee!";

            ViewBag.Action = "PayeeBackToTransaction";
            ViewBag.Controller = "Transaction";

            obj.Payee = "";

            return View("SelectPayee", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchPayee(Transactions obj)
        {

         string SearchString = "%" + obj.Payee + "%" ?? "";

            var Budget = await _db.Budgets
                .Include(x => x.Transactions.Where(t => EF.Functions.Like(t.Payee, SearchString)))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefaultAsync();

            var Payee = Budget.Transactions.Select(t => t.Payee).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToList();

            Payee.Sort();
            if (Payee.Count == 0)
            {
                Payee.Add("No Payees");
            }  

            return PartialView("_PayeeTablePV", Payee);
        }

        public async Task<IActionResult> PayeeTable()
        {

            Budgets? Budget = await _db.Budgets
                .Include(x => x.Transactions)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefaultAsync();

            var Payee = Budget.Transactions.Select(t => t.Payee).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToList();

            Payee.Sort();
            if (Payee.Count == 0)
            {
                Payee.Add("No Payees");
            }

            return PartialView("_PayeeTablePV", Payee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayee(Transactions obj)
        {

            return View("AddTransaction", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Transaction/SelectSpecificPayee/{PayeeName?}")]
        public IActionResult SelectSpecificPayee(Transactions obj, string PayeeName)
        {            

            obj.Payee = PayeeName;

            Budgets? budget = _db.Budgets
                .Include(b => b.Transactions)
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            Transactions? T = budget.Transactions.Where(t => t.Payee == PayeeName & t.Category != "" & !t.isIncome).OrderByDescending(t => t.TransactionID).FirstOrDefault();

            if (T != null)
            {
                obj.Category = T.Category ?? "";
                obj.CategoryID = T.CategoryID;
            }

            return View("AddTransaction", obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Transaction/DeleteSpecificPayee/{PayeeName?}")]
        public IActionResult DeleteSpecificPayee(Transactions obj, string PayeeName)
        {

            Budgets? Budget = _db.Budgets
                .Include(x => x.Transactions.Where(t => t.Payee == PayeeName))
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();


            _db.Attach(Budget);

            foreach (Transactions T in Budget.Transactions)
            {
                T.Payee = "";
                _db.SaveChanges();
            }

            _db.SaveChanges();

            Budget = _db.Budgets
                .Include(x => x.Transactions)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            var Payee = Budget.Transactions.Select(t => t.Payee).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().ToList();


            if (Payee.Count == 0)
            {
                Payee.Add("No Payees");
            }


            Payee.Sort();

            return PartialView("_PayeeTablePV", Payee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayeeBackToTransaction(Transactions obj)
        {
            obj.Payee = "";
            return View("AddTransaction", obj);
        }

                [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryBackToTransaction(Transactions obj)
        {
            
            return View("AddTransaction", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectCategory(Transactions obj)
        {
            Budgets? Budget = _db.Budgets?
                .Include(b =>b.Categories)
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            List<Categories> CategoryList = Budget.Categories.OrderBy(c => c.isSubCategory).ToList(); 

            ViewBag.CategoryList = CategoryList;

            ViewBag.Action = "CategoryBackToTransaction";
            ViewBag.Controller = "Transaction";

            TempData["PageHeading"] = "Select a Category!";

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Transaction/SelectSpecificCategory/{CategoryID?}")]
        public IActionResult SelectSpecificCategory(Transactions obj, int? CategoryID)
        {
            Budgets? Budget = _db.Budgets?
                .Include(b =>b.Categories)
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();
            
            Categories Category = Budget.Categories.Where(c => c.CategoryID == CategoryID).First();

            obj.Category = Category.CategoryName;
            obj.CategoryID = Category.CategoryID;        

            return View("AddTransaction", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSaving(Transactions obj)
        {
            Budgets? Budget = _db.Budgets?
                .Include(b => b.Savings.Where(s => s.isSavingsClosed == false))
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            List<Savings> SavingsList = Budget.Savings.ToList();

            ViewBag.SavingsList = SavingsList;

            ViewBag.Action = "CategoryBackToTransaction";
            ViewBag.Controller = "Transaction";
            ViewBag.TypeSortDirection = "down";
            ViewBag.BalanceSortDirection = "down";
            ViewBag.NameSortDirection = "down";

            TempData["PageHeading"] = "Select a Saving Category!";

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Transaction/SelectSpecificSaving/{SelectSavingID?}")]
        public IActionResult SelectSpecificSaving(Transactions obj, int SelectSavingID)
        {
            Budgets? Budget = _db.Budgets?
                .Include(b => b.Savings)
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            Savings Saving = Budget.Savings.Where(c => c.SavingID == SelectSavingID).First();

            obj.SavingName = Saving.SavingsName;
            obj.SavingID = Saving.SavingID;

            return View("AddTransaction", obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Transaction/SelectSpecificSaving/{direction?}/{sortcolumn?}")]
        public IActionResult SelectSavingSort(Transactions obj, string direction, string sortcolumn)
        {
            Budgets? Budget = _db.Budgets?
                .Include(b => b.Savings.Where(s => s.isSavingsClosed == false))
                .Where(b => b.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            List<Savings> SavingsList = Budget.Savings.ToList();

            if (sortcolumn == "Type")
            {
                if (direction == "up")
                {
                    SavingsList = Budget.Savings.OrderByDescending(s => s.isRegularSaving).ToList();
                    ViewBag.TypeSortDirection = "down";
                    ViewBag.BalanceSortDirection = "down";
                    ViewBag.NameSortDirection = "down";
                }
                else if (direction == "down")
                {
                    SavingsList = Budget.Savings.OrderBy(s => s.isRegularSaving).ToList();
                    ViewBag.TypeSortDirection = "up";
                    ViewBag.BalanceSortDirection = "down";
                    ViewBag.NameSortDirection = "down";
                }
            }
            else if(sortcolumn == "Name")
            {
                if (direction == "up")
                {
                    SavingsList = Budget.Savings.OrderByDescending(s => s.SavingsName).ToList();
                    ViewBag.TypeSortDirection = "down";
                    ViewBag.BalanceSortDirection = "down";
                    ViewBag.NameSortDirection = "down";
                }
                else if (direction == "down")
                {
                    SavingsList = Budget.Savings.OrderBy(s => s.SavingsName).ToList();
                    ViewBag.TypeSortDirection = "down";
                    ViewBag.BalanceSortDirection = "down";
                    ViewBag.NameSortDirection = "up";
                }
            }
            else if (sortcolumn == "Balance")
            {
                if (direction == "up")
                {
                    SavingsList = Budget.Savings.OrderByDescending(s => s.CurrentBalance).ToList();
                    ViewBag.TypeSortDirection = "down";
                    ViewBag.BalanceSortDirection = "down";
                    ViewBag.NameSortDirection = "down";
                }
                else if (direction == "down")
                {
                    SavingsList = Budget.Savings.OrderBy(s => s.CurrentBalance).ToList();
                    ViewBag.TypeSortDirection = "down";
                    ViewBag.BalanceSortDirection = "up";
                    ViewBag.NameSortDirection = "down";
                }
            }

            ViewBag.SavingsList = SavingsList;

            ViewBag.Action = "CategoryBackToTransaction";
            ViewBag.Controller = "Transaction";

            TempData["PageHeading"] = "Select a Saving Category!";

            return View("SelectSaving", obj);
        }
        

    }
}
