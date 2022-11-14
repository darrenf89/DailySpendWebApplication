using System.Drawing.Printing;
using System.Security.Policy;
using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;

namespace DailySpendBudgetWebApp.Services
{
    public interface ISecurityHelper
    {
        public string GenerateSalt(int nSalt);
        public bool CheckUniqueEmail(string Email);
        public string GenerateHashedPassword(string NonHasdedPassword, string Salt, int nIterations, int nHash);
        public bool CheckPassword(string Password, string Email);
        public RegisterModel CreateUserSecurityDetails(RegisterModel obj);
        public bool CheckEmailVerified(string Email);
        public string GenerateEmailToken(UserAccounts obj);
        public bool ValidateEmailToken(string token);
        public string GetClaim(string token, string claimType);

    }
}
