﻿using System.Drawing.Printing;
using System.Globalization;
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
        public string CreateDefaultCategories(int BudgetID);
        public CultureInfo LoadCurrencySetting(int BudgetID);
        public string TransactTransaction(ref Transactions T, int? BudgetID);
        public string TransactSavingsTransaction(ref Transactions T, int? BudgetID);
        public string RecalculateRegularSavingFromTransaction(ref Savings S);
        public string CalculateSavingsTargetAmount(ref Savings S);
        public string CalculateSavingsTargetDate(ref Savings S);
        public string CreateNewPayPeriodStats(int? BudgetID);
        public string GetBudgetDatePattern(int BudgetID);
        public string GetBudgetShortDatePattern(int BudgetID);
        public string UpdatePayPeriodStats(int? BudgetID);
        public string RecalculateBudgetDetails(int? BudgetID);
        public string RegularBudgetUpdateLoop(int? BudgetID);
        public string BudgetUpdateDailyy(int BudgetID, DateTime LastUpdated);
        public void UpdateTransactionDaily(ref Budgets Budget);
        public void TransactSavingsTransactionDaily(ref Budgets Budget, int ID);
        public void TransactTransactionDaily(ref Budgets Budget, int ID);
        public void UpdateSavingsDaily(ref Budgets Budget);
        public void UpdateBillsDaily(ref Budgets Budget);
        public void UpdateIncomesDaily(ref Budgets Budget);



    }
}
