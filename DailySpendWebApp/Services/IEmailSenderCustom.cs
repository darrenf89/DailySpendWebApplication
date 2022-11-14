namespace DailySpendBudgetWebApp.Services
{
    public interface IEmailSenderCustom
    {
        public  Task SendEmailAsync(string toEmail, string subject, string message);

        public  Task Execute(string apiKey, string subject, string message, string toEmail);

        public string GenerateVerifyEmail(string Token);
    }
}
