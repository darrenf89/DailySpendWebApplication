using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class firebaseetc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FireBaseToken",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpiry",
                table: "UserAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetType",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FireBaseToken",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "SubscriptionExpiry",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "Budgets");
        }
    }
}
