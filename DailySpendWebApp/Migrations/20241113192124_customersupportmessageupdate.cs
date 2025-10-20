using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class customersupportmessageupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportMessages_CustomerSupports_SupportID",
                table: "SupportMessages");

            migrationBuilder.DropIndex(
                name: "IX_SupportMessages_SupportID",
                table: "SupportMessages");

            migrationBuilder.DropColumn(
                name: "SupportID",
                table: "SupportMessages");

            migrationBuilder.AddColumn<int>(
                name: "CustomerSupportSupportID",
                table: "SupportMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_CustomerSupportSupportID",
                table: "SupportMessages",
                column: "CustomerSupportSupportID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportMessages_CustomerSupports_CustomerSupportSupportID",
                table: "SupportMessages",
                column: "CustomerSupportSupportID",
                principalTable: "CustomerSupports",
                principalColumn: "SupportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportMessages_CustomerSupports_CustomerSupportSupportID",
                table: "SupportMessages");

            migrationBuilder.DropIndex(
                name: "IX_SupportMessages_CustomerSupportSupportID",
                table: "SupportMessages");

            migrationBuilder.DropColumn(
                name: "CustomerSupportSupportID",
                table: "SupportMessages");

            migrationBuilder.AddColumn<int>(
                name: "SupportID",
                table: "SupportMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_SupportID",
                table: "SupportMessages",
                column: "SupportID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportMessages_CustomerSupports_SupportID",
                table: "SupportMessages",
                column: "SupportID",
                principalTable: "CustomerSupports",
                principalColumn: "SupportID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
