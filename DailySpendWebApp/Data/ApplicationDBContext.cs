using Microsoft.EntityFrameworkCore;
using DailySpendBudgetWebApp.Models;

namespace DailySpendBudgetWebApp.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<UserAccounts>? UserAccounts { get; set; }
        public DbSet<Budgets>? Budgets { get; set; }
        public DbSet<Bills>? Bills { get; set; }
        public DbSet<Categories>? Categories { get; set; }
        public DbSet<IncomeEvents>? IncomeEvents { get; set; }
        public DbSet<Savings>? Savings { get; set; }
        public DbSet<Transactions>? Transactions { get; set; }
        public DbSet<BudgetSettings>? BudgetSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccounts>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
