using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class foreignkeysOTP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OTP_UserAccountID",
                table: "OTP",
                column: "UserAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_OTP_UserAccounts_UserAccountID",
                table: "OTP",
                column: "UserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OTP_UserAccounts_UserAccountID",
                table: "OTP");

            migrationBuilder.DropIndex(
                name: "IX_OTP_UserAccountID",
                table: "OTP");
        }
    }
}
