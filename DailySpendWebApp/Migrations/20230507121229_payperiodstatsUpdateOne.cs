using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class payperiodstatsUpdateOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPeriodStartSpendTotal",
                table: "Budgets");


            migrationBuilder.AddColumn<decimal>(
                name: "EndBBPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EndLtSDailyAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EndLtSPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EndMaBPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartBBPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartLtSDailyAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartLtSPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartMaBPeiordAmount",
                table: "PayPeriodStats",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndBBPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "EndLtSDailyAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "EndLtSPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "EndMaBPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "StartBBPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "StartLtSDailyAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "StartLtSPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.DropColumn(
                name: "StartMaBPeiordAmount",
                table: "PayPeriodStats");

            migrationBuilder.AddColumn<decimal>(
                name: "PayPeriodStartSpendTotal",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true);

        }
    }
}
