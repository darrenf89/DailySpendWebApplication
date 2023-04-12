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
        public List<string?> Stage { get; set; } = new List<string?>();
        public int? SavingID { get; set; }
        public string? SavingsType { get; set; }
        [MaxLength(15)]
        public string? SavingsName { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Current Balance")]
        public decimal? CurrentBalance { get; set; } = 0;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        [DisplayName("When?")]
        public DateTime? GoalDate { get; set; } = null;
        [DataType(DataType.Currency)]
        public decimal? LastUpdatedValue { get; set; }
        public bool? isSavingsClosed { get; set; } = false;
        [DataType(DataType.Currency)]
        [DisplayName("Saving Target")]
        public decimal? SavingsGoal { get; set; } = 0;
        public bool canExceedGoal { get; set; }
        public bool isDailySaving { get; set; }
        public bool isRegularSaving { get; set; }
        [DisplayName("Savings Amount")]
        public decimal? RegularSavingValue { get; set; }
        public bool isAutoComplete { get; set; }
        public int? BillID { get; set; }
        public string? BillName { get; set; }
        public string? BillType { get; set; }
        public int? BillValue { get; set; }
        public string? BillDuration { get; set; }
        public decimal? BillAmount { get; set; }
        public DateTime? BillDueDate { get; set; }
        public decimal? BillCurrentBalance { get; set; } = decimal.Zero;
        public bool? isBillRecuring { get; set; } = false;
        public decimal? RegularBillValue { get; set; }
        public int? AproxDaysBetweenPay { get; set; } = 30;
        public int? IncomeEventID { get; set; }
        public decimal? IncomeAmount { get; set; }
        public string? IncomeName { get; set; } = "";
        public DateTime? IncomeActiveDate { get; set; } = DateTime.Now;
        public DateTime? DateOfIncomeEvent { get; set; } = DateTime.Now;
        public bool? isRecurringIncome { get; set; }
        public string? RecurringIncomeType { get; set; }
        public int? RecurringIncomeValue { get; set; }
        public string? RecurringIncomeDuration { get; set; }
        public bool? isClosed { get; set; } = true;
        public bool? isInstantActive { get; set; }
        public string? ddlSavingsPeriod { get; set; }
        public decimal? PeriodSavingValue { get; set; }

    }
}
