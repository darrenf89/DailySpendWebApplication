using System.Drawing.Printing;
using System.Security.Policy;
using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;

namespace DailySpendWebApp.Services
{
    public interface IProductTools
    {
        public string UpdateBudgetCreateSavings(int BudgetID);
        public string UpdateBudgetCreateIncome(int BudgetID);
        public string UpdateBudgetCreateSavingsSpend(int BudgetID);
        public string UpdateBudgetCreateBillsSpend(int BudgetID);
        public DateTime CalculateNextDate(DateTime LastDate, string Type, int Value, string? Duration);
        public string CalculateNextDateEverynth(ref DateTime NextDate, DateTime LastDate, int Value, string? Duration);
        public string CalculateNextDateWorkingDays(ref DateTime NextDate, DateTime LastDate, int Value);
        public string CalculateNextDateOfEveryMonth(ref DateTime NextDate, DateTime LastDate, int Value);
        public string CalculateNextDateLastOfTheMonth(ref DateTime NextDate, DateTime LastDate, string? Duration);

    }
}
