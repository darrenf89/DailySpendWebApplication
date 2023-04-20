
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

        [ValidateAntiForgeryToken]
        public IActionResult AddTransaction(Transactions obj)
        {

            TempData["PageHeading"] = "Add a Transaction!";
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelelctPayee(Transactions obj)
        {
            Budgets?  Budget = _db.Budgets?
            .Include(x=>x.Transactions.Select(t => t.Payee).Distinct())
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .FirstOrDefault();

            List<string> Payee = new List<string>();
            foreach (Transactions transaction in Budget.Transactions)
            {
                Payee.Add(transaction.Payee ?? "");
            }

            Payee.Sort();
            ViewBag.Payees = Payee;

            return View("SelelctPayee", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchPayee(Transactions obj)
        {

            string SearchString = "%" + obj.Payee + "%" ?? "";

            Budgets?  Budget = _db.Budgets?
            .Include(x=>x.Transactions.Where(t => EF.Functions.Like(t.Payee, SearchString)).Select(t => t.Payee).Distinct())
            .Where(x => x.BudgetID == HttpContext.Session.GetInt32("_DefaultBudgetID"))
            .FirstOrDefault();

            List<string> Payee = new List<string>();
            foreach (Transactions transaction in Budget.Transactions)
            {
                Payee.Add(transaction.Payee);
            }

            Payee.Sort();
            ViewBag.Payees = Payee;

            return View("SelectPayee", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayee(Transactions obj)
        {
            return View("AddTransaction", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Transaction/SelectSpecificPayee/{PayeeName}")]
        public IActionResult SelectSpecificPayee(Transactions obj, string PayeeName)
        {
            obj.Payee = PayeeName;
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
