using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class UserAddDetailsUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddDetails_UserAccounts_UserAccounts",
                table: "UserAddDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserAddDetails_UserAccounts",
                table: "UserAddDetails");

            migrationBuilder.RenameColumn(
                name: "UserAccounts",
                table: "UserAddDetails",
                newName: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserAddDetails",
                newName: "UserAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddDetails_UserAccounts",
                table: "UserAddDetails",
                column: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddDetails_UserAccounts_UserAccounts",
                table: "UserAddDetails",
                column: "UserAccounts",
                principalTable: "UserAccounts",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
