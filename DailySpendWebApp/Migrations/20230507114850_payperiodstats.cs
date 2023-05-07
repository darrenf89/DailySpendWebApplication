using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class payperiodstats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayPeriodStats",
                columns: table => new
                {
                    PayPeriodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isCurrentPeriod = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationOfPeriod = table.Column<int>(type: "int", nullable: false),
                    SavingsToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BillsToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomeToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpendToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriodStats", x => x.PayPeriodID);
                    table.ForeignKey(
                        name: "FK_PayPeriodStats_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriodStats_BudgetsBudgetID",
                table: "PayPeriodStats",
                column: "BudgetsBudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayPeriodStats");
        }
    }
}
