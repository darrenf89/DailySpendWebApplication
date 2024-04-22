using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class dBudgetReleaseDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dBudgetReleaseDetails",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VersionNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMajorVersion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dBudgetReleaseDetails", x => x.ReleaseID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dBudgetReleaseDetails");
        }
    }
}
