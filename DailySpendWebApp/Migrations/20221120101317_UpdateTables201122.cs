using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    public partial class UpdateTables201122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_IncomeEvents_LastIncomeEventId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_LastIncomeEventId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "IncomeEventDuration",
                table: "IncomeEvents");

            migrationBuilder.RenameColumn(
                name: "IncomeType",
                table: "IncomeEvents",
                newName: "IncomeCategory");

            migrationBuilder.RenameColumn(
                name: "NextIncomeEventDate",
                table: "Budgets",
                newName: "NextIncomePayday");

            migrationBuilder.RenameColumn(
                name: "LastIncomeEventId",
                table: "Budgets",
                newName: "PaydayValue");

            migrationBuilder.AddColumn<string>(
                name: "RegularSavingType",
                table: "Savings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RegularSavingValue",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SavingsGoal",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "canExceedGoal",
                table: "Savings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isRegularSaving",
                table: "Savings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaydayAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaydayDuration",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaydayType",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegularSavingType",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "RegularSavingValue",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "SavingsGoal",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "canExceedGoal",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "isRegularSaving",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "PaydayAmount",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "PaydayDuration",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "PaydayType",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "IncomeCategory",
                table: "IncomeEvents",
                newName: "IncomeType");

            migrationBuilder.RenameColumn(
                name: "PaydayValue",
                table: "Budgets",
                newName: "LastIncomeEventId");

            migrationBuilder.RenameColumn(
                name: "NextIncomePayday",
                table: "Budgets",
                newName: "NextIncomeEventDate");

            migrationBuilder.AddColumn<int>(
                name: "IncomeEventDuration",
                table: "IncomeEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_LastIncomeEventId",
                table: "Budgets",
                column: "LastIncomeEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_IncomeEvents_LastIncomeEventId",
                table: "Budgets",
                column: "LastIncomeEventId",
                principalTable: "IncomeEvents",
                principalColumn: "IncomeEventID");
        }
    }
}
