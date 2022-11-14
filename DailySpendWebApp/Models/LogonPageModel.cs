﻿using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class LogonPageModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool isRememberMe { get; set; } = true;

    }
}
