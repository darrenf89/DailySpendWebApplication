using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class BudgetSettinglutTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lut_CurrencyDecimalDigits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyDecimalDigits = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_CurrencyDecimalDigits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_CurrencyDecimalSeparators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyDecimalSeparator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_CurrencyDecimalSeparators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_CurrencyGroupSeparators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyGroupSeparator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_CurrencyGroupSeparators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_CurrencyPlacements",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyPlacement = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_CurrencyPlacements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_DateFormats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSeperatorID = table.Column<int>(type: "int", nullable: false),
                    ShortDatePatternID = table.Column<int>(type: "int", nullable: false),
                    DateFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_DateFormats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_DateSeperators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSeperator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_DateSeperators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_NumberFormats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyDecimalDigitsID = table.Column<int>(type: "int", nullable: false),
                    CurrencyDecimalSeparatorID = table.Column<int>(type: "int", nullable: false),
                    CurrencyGroupSeparatorID = table.Column<int>(type: "int", nullable: false),
                    NumberFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_NumberFormats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lut_ShortDatePatterns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortDatePattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lut_ShortDatePatterns", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lut_CurrencyDecimalDigits");

            migrationBuilder.DropTable(
                name: "lut_CurrencyDecimalSeparators");

            migrationBuilder.DropTable(
                name: "lut_CurrencyGroupSeparators");

            migrationBuilder.DropTable(
                name: "lut_CurrencyPlacements");

            migrationBuilder.DropTable(
                name: "lut_DateFormats");

            migrationBuilder.DropTable(
                name: "lut_DateSeperators");

            migrationBuilder.DropTable(
                name: "lut_NumberFormats");

            migrationBuilder.DropTable(
                name: "lut_ShortDatePatterns");
        }
    }
}
