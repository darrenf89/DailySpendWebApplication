﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models

{
    public class CreateABudgetPageModel
    {

        [Required]
        [DisplayName("Budget Name")]
        [MaxLength(20)]
        [MinLength(5)]
        public string? BudgetName { get; set; }
        public int StartingBalance { get; set; }
        [Required]
        public int BudgetID { get; set; }
        [DisplayName("Currency")]
        public string? CurrencyDDL { get; set; }
        [DisplayName("Number Format")]
        public string? NumberFormatDDL { get; set; }
        [DisplayName("Symbol Placement")]
        public string? CurrencyPlacementDDL { get; set; }
        [DisplayName("Date Format")]
        public string? DateFormatDDL { get; set; }
        [Required]
        [DisplayName("What's in the 'BANK'")]
        public int StartingBalanceInt { get; set; }
        [Required]
        public int StartingBalanceDec { get; set; }
        [Required]
        [DisplayName("When's PAYDAY?")]
        public DateOnly NextIncomeDate { get; set; }
        public string? PeriodicPayDuration { get; set; }
        public int PeriodicPayPeriod { get; set; }
        public int LastDayOfMonthPayPeriod { get; set; }
        public int GivenDayOfMonthPayPeriod { get; set; }
        public string? LastGivenDayOfWeekPay { get; set; }
    }
}
