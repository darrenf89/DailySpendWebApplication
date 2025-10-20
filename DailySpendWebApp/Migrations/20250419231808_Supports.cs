using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class Supports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FamilyUserAccountUserID",
                table: "CustomerSupports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupports_FamilyUserAccountUserID",
                table: "CustomerSupports",
                column: "FamilyUserAccountUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerSupports_FamilyUserAccount_FamilyUserAccountUserID",
                table: "CustomerSupports",
                column: "FamilyUserAccountUserID",
                principalTable: "FamilyUserAccount",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerSupports_FamilyUserAccount_FamilyUserAccountUserID",
                table: "CustomerSupports");

            migrationBuilder.DropIndex(
                name: "IX_CustomerSupports_FamilyUserAccountUserID",
                table: "CustomerSupports");

            migrationBuilder.DropColumn(
                name: "FamilyUserAccountUserID",
                table: "CustomerSupports");
        }
    }
}
