using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class Bills
    {
        [Key]
        public int BillID { get; set; }
        public string? BillName { get; set; }
        public decimal? BillTotalDue { get; set; }
        public decimal? BillRemainingDue { get; set; }
        public DateTime? BillDueDate { get; set; }
        public decimal BillCurrentBalance { get; set; } = decimal.Zero;
        public bool isRecuring { get; set; } = false;
        public bool isPaidInFull { get; set; } = false ;
        public string? RecuringType { get; set; }
        public bool? isRecuringPeriod { get; set; }
        public DateTime? NextRecuringBillDate { get; set; }
        public int? RecuringPeriod { get; set; }

        private DateTime CalcualteNextRecuringBillDate()
        {
            DateTime BillDate = new();

            return BillDate;
        }

    }

    

}
