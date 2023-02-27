using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class Bills
    {
        [Key]
        public int BillID { get; set; }
        public string? BillType { get; set; }
        public string? BillName { get; set; }
        public decimal? BillAmount { get; set; }
        public DateTime? BillDueDate { get; set; }
        public decimal BillCurrentBalance { get; set; } = decimal.Zero;
        public bool isRecuring { get; set; } = false;
        public string? RecuringType { get; set; }
        public int? RecuringPeriod { get; set; }
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public decimal? BillDailyValue { get; set; }
        public decimal? LastUpdatedValue { get; set; }

        private static DateTime CalcualteNextRecuringBillDate()
        {
            DateTime BillDate = new();

            return BillDate;
        }

    }

    

}
