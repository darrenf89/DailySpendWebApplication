using System.ComponentModel.DataAnnotations;


namespace DailySpendBudgetWebApp.Models
{
    public class BankAccounts
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(25)]
        public string? BankAccountName { get; set; }
        [DataType(DataType.Currency)]
        public decimal? AccountBankBalance { get; set; }
        public bool IsDefaultAccount { get; set; }

    }
}
