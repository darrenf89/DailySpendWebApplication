using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class Bills
    {
        [Key]
        public int BillID { get; set; }
        [Required]
        [MaxLength(25)]
        public string? BillName { get; set; }
        [MaxLength(250)]
        public string? BillType { get; set; }
        public int? BillValue { get; set; }
        [MaxLength(250)]
        public string? BillDuration { get; set; }
        [Required]
        public decimal? BillAmount { get; set; }
        [Required]
        public DateTime? BillDueDate { get; set; }
        public decimal BillCurrentBalance { get; set; } = decimal.Zero;
        public bool isRecuring { get; set; } = false;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public bool isClosed { get; set; }
        public decimal? RegularBillValue { get; set; }


    }

    

}
