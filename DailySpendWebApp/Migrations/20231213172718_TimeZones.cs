using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class TimeZones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeZone",
                table: "BudgetSettings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "lut_BudgetTimeZone",
                columns: table => new
                {
                    TimeZoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeZoneName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeZoneUTCOffset = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_BudgetTimeZone", x => x.TimeZoneID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lut_BudgetTimeZone");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "BudgetSettings");
        }
    }
}
