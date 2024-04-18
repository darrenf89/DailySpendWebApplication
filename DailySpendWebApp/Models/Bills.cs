using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Bills
    {
        [Key]
        public int BillID { get; set; }
        [Required]
        [MaxLength(50)]
        public string? BillName { get; set; }
        [MaxLength(25)]
        public string? BillType { get; set; }
        public int? BillValue { get; set; }
        [MaxLength(25)]
        public string? BillDuration { get; set; }
        [Required]
        public decimal? BillAmount { get; set; }
        [Required]
        public DateTime? BillDueDate { get; set; }
        public decimal BillCurrentBalance { get; set; } = decimal.Zero;
        public decimal  BillBalanceAtLastPayDay { get; set; } = decimal.Zero;
        public bool isRecuring { get; set; } = false;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public bool isClosed { get; set; }
        public decimal? RegularBillValue { get; set; }
        [MaxLength(25)]
        public string? BillPayee { get; set; }
        [MaxLength(50)]
        public string? Category { get; set; } = "";
        [ForeignKey("Categories")]
        public int? CategoryID { get; set; }
        public Categories Categories { get; set; }

    }

    

}
