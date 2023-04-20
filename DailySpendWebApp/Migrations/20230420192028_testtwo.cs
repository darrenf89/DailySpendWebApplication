using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class testtwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isSpendFromSavings = table.Column<bool>(type: "bit", nullable: false),
                    SavingID = table.Column<int>(type: "int", nullable: true),
                    SavingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isIncome = table.Column<bool>(type: "bit", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true),
                    CategoriesCategoryID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoriesCategoryID",
                        column: x => x.CategoriesCategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BudgetsBudgetID",
                table: "Transactions",
                column: "BudgetsBudgetID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesCategoryID",
                table: "Transactions",
                column: "CategoriesCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
