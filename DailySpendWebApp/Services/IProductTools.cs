using System.Drawing.Printing;
using System.Security.Policy;
using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;

namespace DailySpendWebApp.Services
{
    public interface IProductTools
    {
        public string UpdateAllBudgetSavings(int BudgetID);
    }
}
