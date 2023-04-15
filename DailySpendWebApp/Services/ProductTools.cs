using DailySpendBudgetWebApp.Models;
using DailySpendBudgetWebApp.Data;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using System.Text;
using DailySpendBudgetWebApp.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DailySpendWebApp.Services
{
    public class ProductTools: IProductTools
    {

        private readonly ApplicationDBContext _db;

        public ProductTools( ApplicationDBContext db)
        {
            _db = db;
        }
        public string UpdateAllBudgetSavings(int BudgetID)
        {
            if (BudgetID == 0)
            {
                return "No Budget Detected";
            }
            else 
            { 

                Budgets? Budget = _db.Budgets?
                    .Include(x => x.Savings.Where(s => s.isSavingsClosed == false))
                    .Where(x => x.BudgetID == BudgetID)
                    .FirstOrDefault();

                int? DaysBetweenPay = Budget.AproxDaysBetweenPay;

                foreach (Savings Saving in Budget.Savings)
                {
                    if(Saving.isRegularSaving)
                    {
                        if (!Saving.isDailySaving)
                        {
                            if(Saving.SavingsType == "TargetAmount")
                            {
                                //Recalculate Date and daily amount
                                Saving.RegularSavingValue = Saving.PeriodSavingValue / DaysBetweenPay;

                                decimal BalanceLeft = Saving.SavingsGoal - Saving.CurrentBalance ?? 0;
                                int NumberOfDays = (int)Math.Ceiling(BalanceLeft / Saving.RegularSavingValue ?? 0);

                                DateTime Today = DateTime.UtcNow;
                                Saving.GoalDate = Today.AddDays(NumberOfDays);

                            }   
                            else if (Saving.SavingsType == "SavingsBuilder")
                            {
                                //Recalculate daily amount.
                                Saving.RegularSavingValue = Saving.PeriodSavingValue / DaysBetweenPay;
                            }
                        }
                    }
                    _db.SaveChanges();
                }
            }
            return "OK"; 
        }
    }
}
