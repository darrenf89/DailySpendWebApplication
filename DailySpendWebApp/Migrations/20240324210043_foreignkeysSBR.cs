using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class foreignkeysSBR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShareBudgetRequest_SharedBudgetID",
                table: "ShareBudgetRequest",
                column: "SharedBudgetID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBudgetRequest_Budgets_SharedBudgetID",
                table: "ShareBudgetRequest",
                column: "SharedBudgetID",
                principalTable: "Budgets",
                principalColumn: "BudgetID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareBudgetRequest_Budgets_SharedBudgetID",
                table: "ShareBudgetRequest");

            migrationBuilder.DropIndex(
                name: "IX_ShareBudgetRequest_SharedBudgetID",
                table: "ShareBudgetRequest");
        }
    }
}
