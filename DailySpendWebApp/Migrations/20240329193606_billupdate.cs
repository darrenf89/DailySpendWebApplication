using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class billupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Bills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Bills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CategoryID",
                table: "Bills",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Categories_CategoryID",
                table: "Bills",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Categories_CategoryID",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_CategoryID",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Bills");
        }
    }
}
