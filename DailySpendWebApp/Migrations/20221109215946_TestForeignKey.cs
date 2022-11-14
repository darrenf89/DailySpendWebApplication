using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class TestForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Budgets_LastIncomeEventId",
                table: "Budgets",
                column: "LastIncomeEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_IncomeEvents_LastIncomeEventId",
                table: "Budgets",
                column: "LastIncomeEventId",
                principalTable: "IncomeEvents",
                principalColumn: "IncomeEventID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_IncomeEvents_LastIncomeEventId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_LastIncomeEventId",
                table: "Budgets");
        }
    }
}
