using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class _230314dbupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IncomeDailyValue",
                table: "IncomeEvents",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "IncomeName",
                table: "IncomeEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecurringIncomePeriod",
                table: "IncomeEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecurringIncomeType",
                table: "IncomeEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "IncomeEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMainIncome",
                table: "IncomeEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isRecurringIncome",
                table: "IncomeEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BudgetValuesLastUpdated",
                table: "Budgets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DailyBillOutgoing",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DailySavingOutgoing",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LeftToSpendDailyAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeDailyValue",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "IncomeName",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "RecurringIncomePeriod",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "RecurringIncomeType",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "isMainIncome",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "isRecurringIncome",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "BudgetValuesLastUpdated",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "DailyBillOutgoing",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "DailySavingOutgoing",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "LeftToSpendDailyAmount",
                table: "Budgets");
        }
    }
}
