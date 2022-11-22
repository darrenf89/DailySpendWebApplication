using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Savings
    {
        [Key]
        public int SavingID { get; set; }
        [Required]
        [MaxLength(15)]
        public string? SavingsName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayName("Current Balance")]
        public decimal? CurrentBalance { get; set; } = 0;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        [DisplayName("When?")]
        public DateTime? GoalDate { get; set; } = null;
        [DataType(DataType.Currency)]
        public decimal? LastUpdatedValue { get; set; }
        public bool isSavingsClosed { get; set; } = false;
        [Required]
        [DataType(DataType.Currency)]
        [DisplayName("Saving Target")]
        public decimal? SavingsGoal { get; set; }
        public bool canExceedGoal { get; set; }
        public bool isRegularSaving { get; set; }
        public string? RegularSavingType { get; set; }
        public decimal? RegularSavingValue { get; set; }

    }
}
