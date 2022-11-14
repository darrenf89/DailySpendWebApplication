using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class budgetSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "budgetSettings",
                columns: table => new
                {
                    SettingsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetID = table.Column<int>(type: "int", nullable: true),
                    CurrencyPattern = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CurrencyDecimalDigits = table.Column<int>(type: "int", nullable: true),
                    CurrencyDecimalSeparator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CurrencyGroupSeparator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DateSeperator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ShortDatePattern = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budgetSettings", x => x.SettingsID);
                    table.ForeignKey(
                        name: "FK_budgetSettings_Budgets_BudgetID",
                        column: x => x.BudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_budgetSettings_BudgetID",
                table: "budgetSettings",
                column: "BudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "budgetSettings");
        }
    }
}
