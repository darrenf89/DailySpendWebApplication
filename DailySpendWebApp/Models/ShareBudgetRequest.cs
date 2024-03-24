﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class ShareBudgetRequest

    {
        [Key]
        public int SharedBudgetRequestID { get; set; }
        public int SharedBudgetID { get; set; }
        public int SharedWithUserAccountID { get; set; }
        [MaxLength(40)]
        public string? SharedWithUserEmail { get; set; }
        [MaxLength(40)]
        public string? SharedByUserEmail { get; set; }
        public bool IsVerified { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RequestInitiated { get; set; } = DateTime.UtcNow;


    }
}