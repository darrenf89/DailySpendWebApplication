using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class mssqlazure_migration_612 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoggedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDPAPermissions = table.Column<bool>(type: "bit", nullable: false),
                    DefaultBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MoneyAvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeftToSPendBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastIncomeEventId = table.Column<int>(type: "int", nullable: true),
                    NextIncomeEventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserAccountsUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetID);
                    table.ForeignKey(
                        name: "FK_Budgets_UserAccounts_UserAccountsUserID",
                        column: x => x.UserAccountsUserID,
                        principalTable: "UserAccounts",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillTotalDue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BillRemainingDue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BillDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BillCurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isRecuring = table.Column<bool>(type: "bit", nullable: false),
                    isPaidInFull = table.Column<bool>(type: "bit", nullable: false),
                    RecuringType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isRecuringPeriod = table.Column<bool>(type: "bit", nullable: true),
                    NextRecuringBillDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecuringPeriod = table.Column<int>(type: "int", nullable: true),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillID);
                    table.ForeignKey(
                        name: "FK_Bills_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCategorySpend = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastTransactionID = table.Column<int>(type: "int", nullable: true),
                    NumberOfTransactions = table.Column<int>(type: "int", nullable: true),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateTable(
                name: "IncomeEvents",
                columns: table => new
                {
                    IncomeEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateOfIncomeEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncomeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomeEventDuration = table.Column<int>(type: "int", nullable: false),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeEvents", x => x.IncomeEventID);
                    table.ForeignKey(
                        name: "FK_IncomeEvents_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateTable(
                name: "Savings",
                columns: table => new
                {
                    SavingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavingsName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    isSavingsClosed = table.Column<bool>(type: "bit", nullable: false),
                    BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Savings", x => x.SavingID);
                    table.ForeignKey(
                        name: "FK_Savings_Budgets_BudgetsBudgetID",
                        column: x => x.BudgetsBudgetID,
                        principalTable: "Budgets",
                        principalColumn: "BudgetID");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isSpendFromSavings = table.Column<bool>(type: "bit", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "IX_Bills_BudgetsBudgetID",
                table: "Bills",
                column: "BudgetsBudgetID");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserAccountsUserID",
                table: "Budgets",
                column: "UserAccountsUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BudgetsBudgetID",
                table: "Categories",
                column: "BudgetsBudgetID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEvents_BudgetsBudgetID",
                table: "IncomeEvents",
                column: "BudgetsBudgetID");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_BudgetsBudgetID",
                table: "Savings",
                column: "BudgetsBudgetID");

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
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "IncomeEvents");

            migrationBuilder.DropTable(
                name: "Savings");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
