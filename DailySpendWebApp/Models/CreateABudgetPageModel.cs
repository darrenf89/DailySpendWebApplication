using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models

{
    public class CreateABudgetPageModel
    {

        [DisplayName("Budget Name")]
        [MaxLength(20)]
        [MinLength(5)]
        public string? BudgetName { get; set; }
        [DisplayName("What's in the 'BANK'")]
        public string? StartingBalance { get; set; }
        [DisplayName("How much do you get paid?!")]
        public string? NextPayDayAmount { get; set; }
        public int? BudgetID { get; set; }
        [DisplayName("Currency")]
        public string? CurrencyDDL { get; set; }
        [DisplayName("Number Format")]
        public string? NumberFormatDDL { get; set; }
        [DisplayName("Symbol Placement")]
        public string? CurrencyPlacementDDL { get; set; }
        [DisplayName("Date Format")]
        public string? DateFormatDDL { get; set; }
        [DisplayName("When's PAYDAY?")]
        public string? NextIncomeDate { get; set; }
        public string? PeriodicPayDuration { get; set; }
        public int? PeriodicPayPeriod { get; set; }
        public string? PeriodicPayPeriodDDL { get; set; }
        public int? LastDayOfMonthPayPeriod { get; set; }
        public int? GivenDayOfMonthPayPeriod { get; set; }
        public string? LastGivenDayOfWeekPay { get; set; }
        public bool? Everynth { get; set; }
        public bool? WorkingDays { get; set; }
        public bool? OfEveryMonth { get; set; }
        public bool? LastOfTheMonth { get; set; }
        

    }
}
