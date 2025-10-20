using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class BankAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AccountBankBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDefaultAccount = table.Column<bool>(type: "bit", nullable: false),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BudgetsBudgetID",
                table: "BankAccounts",
                column: "BudgetsBudgetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
