using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class BillsUpadte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextRecuringBillDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "isPaidInFull",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "isRecuringPeriod",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "BillTotalDue",
                table: "Bills",
                newName: "LastUpdatedValue");

            migrationBuilder.RenameColumn(
                name: "BillRemainingDue",
                table: "Bills",
                newName: "BillDailyValue");

            migrationBuilder.AddColumn<decimal>(
                name: "BillAmount",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillType",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillAmount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedValue",
                table: "Bills",
                newName: "BillTotalDue");

            migrationBuilder.RenameColumn(
                name: "BillDailyValue",
                table: "Bills",
                newName: "BillRemainingDue");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRecuringBillDate",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isPaidInFull",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isRecuringPeriod",
                table: "Bills",
                type: "bit",
                nullable: true);
        }
    }
}
