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
            Budgets? Budget = _db.Budgets?
                .Include(x => x.Savings .Where(s => s.isSavingsClosed == false))
                .Where(x => x.BudgetID == BudgetID)
                .FirstOrDefault();

            foreach (Savings Saving in Budget.Savings)
            {

            }

            return "OK"; 
        }
    }
}
