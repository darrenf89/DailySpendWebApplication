using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class incomeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeDailyValue",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "IncomeEvents");

            migrationBuilder.RenameColumn(
                name: "isMainIncome",
                table: "IncomeEvents",
                newName: "isClosed");

            migrationBuilder.RenameColumn(
                name: "IncomeCategory",
                table: "IncomeEvents",
                newName: "RecurringIncomeDuration");

            migrationBuilder.AlterColumn<string>(
                name: "RecurringIncomeType",
                table: "IncomeEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RecurringIncomePeriod",
                table: "IncomeEvents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "IncomeActiveDate",
                table: "IncomeEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isInstantActive",
                table: "IncomeEvents",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeActiveDate",
                table: "IncomeEvents");

            migrationBuilder.DropColumn(
                name: "isInstantActive",
                table: "IncomeEvents");

            migrationBuilder.RenameColumn(
                name: "isClosed",
                table: "IncomeEvents",
                newName: "isMainIncome");

            migrationBuilder.RenameColumn(
                name: "RecurringIncomeDuration",
                table: "IncomeEvents",
                newName: "IncomeCategory");

            migrationBuilder.AlterColumn<string>(
                name: "RecurringIncomeType",
                table: "IncomeEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RecurringIncomePeriod",
                table: "IncomeEvents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IncomeDailyValue",
                table: "IncomeEvents",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "IncomeEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
