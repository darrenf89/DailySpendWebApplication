using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class addingPreviousDayDailyAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "StartDayDailyAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartLtSDailyAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDayDailyAmount",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "StartLtSDailyAmount",
                table: "Budgets");
        }
    }
}
