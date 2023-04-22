using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class categoryupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CategoriesCategoryID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LastTransactionID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "NumberOfTransactions",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TotalCategorySpend",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "SavingName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Payee",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryGroupID",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isSubCategory",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryGroupID",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "isSubCategory",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "SavingName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Payee",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategoryID",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastTransactionID",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTransactions",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCategorySpend",
                table: "Categories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesCategoryID",
                table: "Transactions",
                column: "CategoriesCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryID",
                table: "Transactions",
                column: "CategoriesCategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }
    }
}
