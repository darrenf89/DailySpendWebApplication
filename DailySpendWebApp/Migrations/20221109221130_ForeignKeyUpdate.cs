using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class ForeignKeyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_DefaultBudgetID",
                table: "UserAccounts",
                column: "DefaultBudgetID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryID",
                table: "Transactions",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryID",
                table: "Transactions",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Budgets_DefaultBudgetID",
                table: "UserAccounts",
                column: "DefaultBudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryID",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Budgets_DefaultBudgetID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_DefaultBudgetID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategoryID",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesCategoryID",
                table: "Transactions",
                column: "CategoriesCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryID",
                table: "Transactions",
                column: "CategoriesCategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }
    }
}
