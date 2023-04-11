using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class incomeeventupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecurringIncomePeriod",
                table: "IncomeEvents",
                newName: "RecurringIncomeValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecurringIncomeValue",
                table: "IncomeEvents",
                newName: "RecurringIncomePeriod");
        }
    }
}
