using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class FamilyAccountBudgetID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FamilyUserAccountUserID",
                table: "Budgets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_FamilyUserAccountUserID",
                table: "Budgets",
                column: "FamilyUserAccountUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_FamilyUserAccount_FamilyUserAccountUserID",
                table: "Budgets",
                column: "FamilyUserAccountUserID",
                principalTable: "FamilyUserAccount",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_FamilyUserAccount_FamilyUserAccountUserID",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_FamilyUserAccountUserID",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "FamilyUserAccountUserID",
                table: "Budgets");
        }
    }
}
