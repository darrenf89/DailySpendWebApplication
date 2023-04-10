using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using DailySpendBudgetWebApp.Controllers;

namespace DailySpendWebApp.Controllers
{
    public class IncomesController : Controller
    {
        private readonly ILogger<IncomesController> _logger;

        private readonly ApplicationDBContext _db;
        public IncomesController(ILogger<IncomesController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult AddIncome()
        {
            return View();
        }

    }
}
