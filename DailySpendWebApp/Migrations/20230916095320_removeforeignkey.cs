using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class removeforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Budgets_DefaultBudgetID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_DefaultBudgetID",
                table: "UserAccounts");

            migrationBuilder.AddColumn<int>(
                name: "BudgetID",
                table: "UserAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_BudgetID",
                table: "UserAccounts",
                column: "BudgetID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Budgets_BudgetID",
                table: "UserAccounts",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Budgets_BudgetID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_BudgetID",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "BudgetID",
                table: "UserAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_DefaultBudgetID",
                table: "UserAccounts",
                column: "DefaultBudgetID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Budgets_DefaultBudgetID",
                table: "UserAccounts",
                column: "DefaultBudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID");
        }
    }
}
