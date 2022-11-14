using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class budgetSettingsTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgetSettings_Budgets_BudgetID",
                table: "budgetSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_budgetSettings",
                table: "budgetSettings");

            migrationBuilder.RenameTable(
                name: "budgetSettings",
                newName: "BudgetSettings");

            migrationBuilder.RenameIndex(
                name: "IX_budgetSettings_BudgetID",
                table: "BudgetSettings",
                newName: "IX_BudgetSettings_BudgetID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetSettings",
                table: "BudgetSettings",
                column: "SettingsID");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetSettings_Budgets_BudgetID",
                table: "BudgetSettings",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetSettings_Budgets_BudgetID",
                table: "BudgetSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetSettings",
                table: "BudgetSettings");

            migrationBuilder.RenameTable(
                name: "BudgetSettings",
                newName: "budgetSettings");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetSettings_BudgetID",
                table: "budgetSettings",
                newName: "IX_budgetSettings_BudgetID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_budgetSettings",
                table: "budgetSettings",
                column: "SettingsID");

            migrationBuilder.AddForeignKey(
                name: "FK_budgetSettings_Budgets_BudgetID",
                table: "budgetSettings",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID");
        }
    }
}
