using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class billupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillDailyValue",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "LeftToSPendBalance",
                table: "Budgets",
                newName: "LeftToSpendBalance");

            migrationBuilder.RenameColumn(
                name: "RecuringType",
                table: "Bills",
                newName: "BillDuration");

            migrationBuilder.RenameColumn(
                name: "RecuringPeriod",
                table: "Bills",
                newName: "BillValue");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedValue",
                table: "Bills",
                newName: "RegularBillValue");

            migrationBuilder.AddColumn<bool>(
                name: "isClosed",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "LeftToSpendBalance",
                table: "Budgets",
                newName: "LeftToSPendBalance");

            migrationBuilder.RenameColumn(
                name: "RegularBillValue",
                table: "Bills",
                newName: "LastUpdatedValue");

            migrationBuilder.RenameColumn(
                name: "BillValue",
                table: "Bills",
                newName: "RecuringPeriod");

            migrationBuilder.RenameColumn(
                name: "BillDuration",
                table: "Bills",
                newName: "RecuringType");

            migrationBuilder.AddColumn<decimal>(
                name: "BillDailyValue",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
