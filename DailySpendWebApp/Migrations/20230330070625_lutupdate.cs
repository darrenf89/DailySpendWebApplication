using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class lutupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "lut_CurrencySymbols",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "lut_CurrencySymbols",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "lut_CurrencySymbols");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "lut_CurrencySymbols");
        }
    }
}
