
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

            var Payee = Budget.Transactions.Select(t => t.Payee).Distinct().ToList();

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

            var Payee = Budget.Transactions.Select(t => t.Payee).Distinct().ToList();

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

            Budgets? Budget = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();

            _db.Attach(Budget);
            Budget.Transactions.Add(obj);

            _db.SaveChanges();

            return View("AddTransaction", obj);
        }

        [HttpPost]
        [Route("Transaction/SelectSpecificPayee/{PayeeName?}")]
        public IActionResult SelectSpecificPayee(Transactions obj, string PayeeName)
        {
            obj.Payee = PayeeName;

            Budgets? Budget = _db.Budgets
                .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
                .FirstOrDefault();


            _db.Attach(Budget);
            Budget.Transactions.Add(obj);

            _db.SaveChanges();

            return View("AddTransaction", obj);
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
