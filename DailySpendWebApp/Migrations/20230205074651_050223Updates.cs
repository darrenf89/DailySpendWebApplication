using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class _050223Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegularSavingType",
                table: "Savings",
                newName: "SavingsType");

            migrationBuilder.AlterColumn<decimal>(
                name: "SavingsGoal",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentBalance",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "isDailySaving",
                table: "Savings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "AproxDaysBetweenPay",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDailySaving",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "AproxDaysBetweenPay",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "SavingsType",
                table: "Savings",
                newName: "RegularSavingType");

            migrationBuilder.AlterColumn<decimal>(
                name: "SavingsGoal",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentBalance",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
