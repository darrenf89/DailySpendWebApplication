using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class _050223Updates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AproxDaysBetweenPay",
                table: "Budgets",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AproxDaysBetweenPay",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
