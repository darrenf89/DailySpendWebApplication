using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class foreignkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShareBudgetRequest_SharedWithUserAccountID",
                table: "ShareBudgetRequest",
                column: "SharedWithUserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePictureImages_UserID",
                table: "ProfilePictureImages",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictureImages_UserAccounts_UserID",
                table: "ProfilePictureImages",
                column: "UserID",
                principalTable: "UserAccounts",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBudgetRequest_UserAccounts_SharedWithUserAccountID",
                table: "ShareBudgetRequest",
                column: "SharedWithUserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictureImages_UserAccounts_UserID",
                table: "ProfilePictureImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBudgetRequest_UserAccounts_SharedWithUserAccountID",
                table: "ShareBudgetRequest");

            migrationBuilder.DropIndex(
                name: "IX_ShareBudgetRequest_SharedWithUserAccountID",
                table: "ShareBudgetRequest");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePictureImages_UserID",
                table: "ProfilePictureImages");
        }
    }
}
