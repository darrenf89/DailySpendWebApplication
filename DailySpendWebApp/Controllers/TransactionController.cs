
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

            var Transactions = Budget.Transactions.Select(t => t.Payee).Distinct();

            List<string> Payee = new List<string>();
            foreach (string payee in Transactions)
            {
                Payee.Add(payee ?? "");
            }

            Payee.Sort();
            if (Payee.Count == 0) 
            {
                Payee.Add("No Payees");    
            }
            obj.PayeeList = Payee;
            TempData["PageHeading"] = "Select a Payee!";
            return View("SelectPayee", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchPayee(Transactions obj)
        {

            string SearchString = "%" + obj.Payee + "%" ?? "";

            List<string> Payee = obj.PayeeList.Where(payee => EF.Functions.Like(payee, SearchString)).Select(payee => payee).Distinct().ToList();

            Payee.Sort();
            if (Payee.Count == 0)
            {
                Payee.Add("No Payees");
            }
            
            obj.PayeeList = Payee;
            TempData["PageHeading"] = "Select a Payee!";
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
