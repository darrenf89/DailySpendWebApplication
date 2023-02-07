using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class SavingUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAutoComplete",
                table: "Savings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAutoComplete",
                table: "Savings");
        }
    }
}
