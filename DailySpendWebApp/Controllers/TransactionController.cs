
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
        public IActionResult AddTransaction(Transactions obj)
        {
            TempData["PageHeading"] = "Add a Transaction!";
            return View(obj);
        }
    }
}
