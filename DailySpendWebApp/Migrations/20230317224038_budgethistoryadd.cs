using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class budgethistoryadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetHstoryLastPeriod",
                columns: table => new
                {
                    BudgetHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MoneyAvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeftToSPendBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalEarnedLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSpentLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSavedlastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalBillsLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetHstoryLastPeriod", x => x.BudgetHistoryID);
                    table.ForeignKey(
                        name: "FK_BudgetHstoryLastPeriod_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetHstoryLastPeriod_BudgetsBudgetID",
                table: "BudgetHstoryLastPeriod",
                column: "BudgetsBudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetHstoryLastPeriod");
        }
    }
}
