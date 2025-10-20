﻿using DailySpendWebApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Budgets
    {
        [Key]
        public int BudgetID { get; set; }
        [MaxLength(50)]
        public string? BudgetName { get; set; }
        public DateTime BudgetCreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Currency)]
        public decimal? BankBalance { get; set; }
        [DataType(DataType.Currency)]
        public decimal? MoneyAvailableBalance { get; set; }        
        [DataType(DataType.Currency)]
        public decimal? PlusStashSpendBalance { get; set; }
        [DataType(DataType.Currency)]
        public decimal? BalanceAllocatedToAllowance { get; set; }
        [DataType(DataType.Currency)]
        public decimal? LeftToSpendBalance { get; set; }
        public DateTime? NextIncomePayday { get; set; }
        public DateTime? NextIncomePaydayCalculated { get; set; }
        public decimal? PaydayAmount { get; set; }
        [MaxLength(25)]
        public string? PaydayType { get; set; }
        public int? PaydayValue { get; set; }
        [MaxLength(25)]
        public string? PaydayDuration { get; set; }
        public bool IsCreated { get; set; }
        public bool IsMultipleAccounts { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<IncomeEvents> IncomeEvents { get; set; } = new List<IncomeEvents>();
        public List<Savings> Savings { get; set; } = new List<Savings>();
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public List<Categories> Categories { get; set; } = new List<Categories>();
        public List<Bills> Bills { get; set; } = new List<Bills>();
        public List<PayPeriodStats> PayPeriodStats { get; set; } = new List<PayPeriodStats>();
        public List<BudgetHstoryLastPeriod> BudgetHistory { get; set; } = new List<BudgetHstoryLastPeriod>();        
        public List<BankAccounts> BankAccounts { get; set; } = new List<BankAccounts>();
        public string? CurrencyType { get; set; }
        [DataType(DataType.Currency)]
        public int? AproxDaysBetweenPay { get; set; } = 30;
        public DateTime BudgetValuesLastUpdated { get; set; } = DateTime.UtcNow;
        public decimal DailySavingOutgoing { get; set; }
        public decimal DailyBillOutgoing { get; set; }
        public decimal LeftToSpendDailyAmount { get; set; }
        public decimal? StartDayDailyAmount { get; set; }
        public int Stage { get; set; } = 1;
        public int SharedUserID { get; set; } = 0;
        public bool IsSharedValidated { get; set; } = false;
        [MaxLength(15)]
        public string BudgetType { get; set; } = "Basic";
        [DefaultValue(true)]
        public bool IsBorrowPay { get; set; } = true;
        [DefaultValue(0)]
        public decimal CurrentActiveIncome { get; set; } = 0;
    }
}
