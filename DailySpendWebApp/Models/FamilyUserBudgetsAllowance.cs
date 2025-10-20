using System.ComponentModel.DataAnnotations;

namespace DailySpendWebApp.Models
{
    public class FamilyUserBudgetsAllowance
    {
        [Key]
        public int AllowancePaymentID { get; set; }
        public int ParentUserID { get; set; }
        public int FamilyUserID { get; set; }        
        public int ParentBudgetID { get; set; }
        public DateTime AllowancePaymentDate { get; set; }
        public double AllowancePaymentAmount { get; set; }
        public bool IsParentAdded { get; set; }

    }
}
