using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class Reset : Migration
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

            //migrationBuilder.CreateTable(
            //    name: "lut_CurrencyDecimalSeparators",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencyDecimalSeparator = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_CurrencyDecimalSeparators", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_CurrencyGroupSeparators",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencyGroupSeparator = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_CurrencyGroupSeparators", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_CurrencyPlacements",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencyPlacement = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_CurrencyPlacements", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_CurrencySymbols",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencySymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_CurrencySymbols", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_DateFormats",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DateSeperatorID = table.Column<int>(type: "int", nullable: false),
            //        ShortDatePatternID = table.Column<int>(type: "int", nullable: false),
            //        DateFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_DateFormats", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_DateSeperators",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DateSeperator = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_DateSeperators", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_NumberFormats",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencyDecimalDigitsID = table.Column<int>(type: "int", nullable: false),
            //        CurrencyDecimalSeparatorID = table.Column<int>(type: "int", nullable: false),
            //        CurrencyGroupSeparatorID = table.Column<int>(type: "int", nullable: false),
            //        NumberFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_NumberFormats", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "lut_ShortDatePatterns",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ShortDatePattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_lut_ShortDatePatterns", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Bills",
            //    columns: table => new
            //    {
            //        BillID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        BillType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BillValue = table.Column<int>(type: "int", nullable: true),
            //        BillDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BillAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        BillDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        BillCurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        isRecuring = table.Column<bool>(type: "bit", nullable: false),
            //        LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        isClosed = table.Column<bool>(type: "bit", nullable: false),
            //        RegularBillValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Bills", x => x.BillID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BudgetHstoryLastPeriod",
            //    columns: table => new
            //    {
            //        BudgetHistoryID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        BankBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        MoneyAvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        LeftToSPendBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        TotalEarnedLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        TotalSpentLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        TotalSavedlastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        TotalBillsLastPeriod = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetHstoryLastPeriod", x => x.BudgetHistoryID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Budgets",
            //    columns: table => new
            //    {
            //        BudgetID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BudgetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BudgetCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        BankBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        MoneyAvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        LeftToSpendBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        NextIncomePayday = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        PaydayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        PaydayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PaydayValue = table.Column<int>(type: "int", nullable: true),
            //        PaydayDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CurrencyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AproxDaysBetweenPay = table.Column<int>(type: "int", nullable: true),
            //        BudgetValuesLastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        DailySavingOutgoing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        DailyBillOutgoing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        LeftToSpendDailyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        UserAccountsUserID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Budgets", x => x.BudgetID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BudgetSettings",
            //    columns: table => new
            //    {
            //        SettingsID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BudgetID = table.Column<int>(type: "int", nullable: true),
            //        CurrencyPattern = table.Column<int>(type: "int", maxLength: 5, nullable: true),
            //        CurrencySymbol = table.Column<int>(type: "int", maxLength: 5, nullable: true),
            //        CurrencyDecimalDigits = table.Column<int>(type: "int", nullable: true),
            //        CurrencyDecimalSeparator = table.Column<int>(type: "int", maxLength: 5, nullable: true),
            //        CurrencyGroupSeparator = table.Column<int>(type: "int", maxLength: 5, nullable: true),
            //        DateSeperator = table.Column<int>(type: "int", maxLength: 5, nullable: true),
            //        ShortDatePattern = table.Column<int>(type: "int", maxLength: 10, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BudgetSettings", x => x.SettingsID);
            //        table.ForeignKey(
            //            name: "FK_BudgetSettings_Budgets_BudgetID",
            //            column: x => x.BudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        CategoryID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TotalCategorySpend = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        LastTransactionID = table.Column<int>(type: "int", nullable: true),
            //        NumberOfTransactions = table.Column<int>(type: "int", nullable: true),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.CategoryID);
            //        table.ForeignKey(
            //            name: "FK_Categories_Budgets_BudgetsBudgetID",
            //            column: x => x.BudgetsBudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "IncomeEvents",
            //    columns: table => new
            //    {
            //        IncomeEventID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IncomeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        IncomeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IncomeActiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        DateOfIncomeEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        isRecurringIncome = table.Column<bool>(type: "bit", nullable: false),
            //        RecurringIncomeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RecurringIncomeValue = table.Column<int>(type: "int", nullable: true),
            //        RecurringIncomeDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        isClosed = table.Column<bool>(type: "bit", nullable: false),
            //        isInstantActive = table.Column<bool>(type: "bit", nullable: true),
            //        isIncomeAddedToBalance = table.Column<bool>(type: "bit", nullable: true),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_IncomeEvents", x => x.IncomeEventID);
            //        table.ForeignKey(
            //            name: "FK_IncomeEvents_Budgets_BudgetsBudgetID",
            //            column: x => x.BudgetsBudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Savings",
            //    columns: table => new
            //    {
            //        SavingID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SavingsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SavingsName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            //        CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        GoalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        LastUpdatedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        isSavingsClosed = table.Column<bool>(type: "bit", nullable: false),
            //        SavingsGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        canExceedGoal = table.Column<bool>(type: "bit", nullable: false),
            //        isDailySaving = table.Column<bool>(type: "bit", nullable: false),
            //        isRegularSaving = table.Column<bool>(type: "bit", nullable: false),
            //        RegularSavingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        PeriodSavingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        isAutoComplete = table.Column<bool>(type: "bit", nullable: false),
            //        ddlSavingsPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Savings", x => x.SavingID);
            //        table.ForeignKey(
            //            name: "FK_Savings_Budgets_BudgetsBudgetID",
            //            column: x => x.BudgetsBudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserAccounts",
            //    columns: table => new
            //    {
            //        UserID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        NickName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        AccountCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LastLoggedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        isDPAPermissions = table.Column<bool>(type: "bit", nullable: false),
            //        isAgreedToTerms = table.Column<bool>(type: "bit", nullable: false),
            //        isEmailVerified = table.Column<bool>(type: "bit", nullable: false),
            //        DefaultBudgetID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserAccounts", x => x.UserID);
            //        table.ForeignKey(
            //            name: "FK_UserAccounts_Budgets_DefaultBudgetID",
            //            column: x => x.DefaultBudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Transactions",
            //    columns: table => new
            //    {
            //        TransactionID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        isSpendFromSavings = table.Column<bool>(type: "bit", nullable: false),
            //        SavingID = table.Column<int>(type: "int", nullable: true),
            //        SavingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        isIncome = table.Column<bool>(type: "bit", nullable: false),
            //        TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Payee = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CategoryID = table.Column<int>(type: "int", nullable: false),
            //        BudgetsBudgetID = table.Column<int>(type: "int", nullable: true),
            //        CategoriesCategoryID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Transactions", x => x.TransactionID);
            //        table.ForeignKey(
            //            name: "FK_Transactions_Budgets_BudgetsBudgetID",
            //            column: x => x.BudgetsBudgetID,
            //            principalTable: "Budgets",
            //            principalColumn: "BudgetID");
            //        table.ForeignKey(
            //            name: "FK_Transactions_Categories_CategoriesCategoryID",
            //            column: x => x.CategoriesCategoryID,
            //            principalTable: "Categories",
            //            principalColumn: "CategoryID");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Bills_BudgetsBudgetID",
            //    table: "Bills",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BudgetHstoryLastPeriod_BudgetsBudgetID",
            //    table: "BudgetHstoryLastPeriod",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Budgets_UserAccountsUserID",
            //    table: "Budgets",
            //    column: "UserAccountsUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BudgetSettings_BudgetID",
            //    table: "BudgetSettings",
            //    column: "BudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Categories_BudgetsBudgetID",
            //    table: "Categories",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_IncomeEvents_BudgetsBudgetID",
            //    table: "IncomeEvents",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Savings_BudgetsBudgetID",
            //    table: "Savings",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transactions_BudgetsBudgetID",
            //    table: "Transactions",
            //    column: "BudgetsBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transactions_CategoriesCategoryID",
            //    table: "Transactions",
            //    column: "CategoriesCategoryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserAccounts_DefaultBudgetID",
            //    table: "UserAccounts",
            //    column: "DefaultBudgetID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserAccounts_Email",
            //    table: "UserAccounts",
            //    column: "Email",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Bills_Budgets_BudgetsBudgetID",
            //    table: "Bills",
            //    column: "BudgetsBudgetID",
            //    principalTable: "Budgets",
            //    principalColumn: "BudgetID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_BudgetHstoryLastPeriod_Budgets_BudgetsBudgetID",
            //    table: "BudgetHstoryLastPeriod",
            //    column: "BudgetsBudgetID",
            //    principalTable: "Budgets",
            //    principalColumn: "BudgetID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Budgets_UserAccounts_UserAccountsUserID",
            //    table: "Budgets",
            //    column: "UserAccountsUserID",
            //    principalTable: "UserAccounts",
            //    principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Budgets_DefaultBudgetID",
                table: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BudgetHstoryLastPeriod");

            migrationBuilder.DropTable(
                name: "BudgetSettings");

            migrationBuilder.DropTable(
                name: "IncomeEvents");

            migrationBuilder.DropTable(
                name: "lut_CurrencyDecimalDigits");

            migrationBuilder.DropTable(
                name: "lut_CurrencyDecimalSeparators");

            migrationBuilder.DropTable(
                name: "lut_CurrencyGroupSeparators");

            migrationBuilder.DropTable(
                name: "lut_CurrencyPlacements");

            migrationBuilder.DropTable(
                name: "lut_CurrencySymbols");

            migrationBuilder.DropTable(
                name: "lut_DateFormats");

            migrationBuilder.DropTable(
                name: "lut_DateSeperators");

            migrationBuilder.DropTable(
                name: "lut_NumberFormats");

            migrationBuilder.DropTable(
                name: "lut_ShortDatePatterns");

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
