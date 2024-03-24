using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class Updatetoby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SharedToUserEmail",
                table: "ShareBudgetRequest",
                newName: "SharedByUserEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SharedByUserEmail",
                table: "ShareBudgetRequest",
                newName: "SharedToUserEmail");
        }
    }
}
