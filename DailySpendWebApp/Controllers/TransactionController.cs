
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

        public IActionResult AddTransaction(Transactions obj)
        {
            Budgets? Budget = _db.Budgets?
                .Include(x => x.Transactions)
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            _db.Attach(Budget);

            Transactions T = new Transactions();

            Budget.Transactions.Add(T);

            _db.SaveChanges();

            obj = T;            

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

            Transactions? T = _db.Transactions
                .Where(x => x.TransactionID == obj.TransactionID)
                .FirstOrDefault();

            _db.Attach(T);

            T.Payee = obj.Payee;

            _db.SaveChanges();

            return View("AddTransaction", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Transaction/SelectSpecificPayee/{PayeeName?}")]
        public IActionResult SelectSpecificPayee(Transactions obj, string PayeeName)
        {            

            obj.Payee = PayeeName;

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
                if (!T.isTransactionCreated & T.TransactionID != obj.TransactionID)
                {
                    _db.Remove(T);
                }
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

        

    }
}
