﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DailySpendBudgetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailySpendBudgetWebApp.Data;

namespace DailySpendBudgetWebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDBContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        UserHomePageModel obj = new();
        try
        {
            obj.Email = User.FindFirst(ClaimTypes.Name).Value;
            var Users = _db.UserAccounts
                .Where(x => x.Email == obj.Email);
            UserAccounts? UserAccount = Users.FirstOrDefault();
            if (UserAccount.NickName == null || UserAccount.NickName == "")
            {
                obj.NickName = "My Fav User";
            }
            else
            { 
                obj.NickName = UserAccount?.NickName;
            }

            HttpContext.Session.SetString("_Email",UserAccount.Email);
            HttpContext.Session.SetString("_Name", UserAccount.NickName);
            HttpContext.Session.SetInt32("_UserID", UserAccount.UserID);
            

            if (UserAccount.DefaultBudgetID.HasValue)
            {
                HttpContext.Session.SetInt32("_DefaultBudgetID", (int)UserAccount.DefaultBudgetID);
                TempData["PageHeading"] = "Good Morning " + obj.NickName;
                TempData["NickName"] = HttpContext.Session.GetString("_Name");
                return View(obj);
            }
            else
            {
                //return View(obj);
                return RedirectToAction("CreateFirstBudget", "Budgets");
            }

        }
        catch
        {
            return RedirectToAction("Error", "Shared");
        }


    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}