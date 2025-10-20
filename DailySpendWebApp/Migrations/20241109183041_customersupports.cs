using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class customersupports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSupports",
                columns: table => new
                {
                    SupportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Whenadded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    UserAccountsUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupports", x => x.SupportID);
                    table.ForeignKey(
                        name: "FK_CustomerSupports_UserAccounts_UserAccountsUserID",
                        column: x => x.UserAccountsUserID,
                        principalTable: "UserAccounts",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SupportMessages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Whenadded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomerReply = table.Column<bool>(type: "bit", nullable: false),
                    CustomerSupportSupportID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportMessages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_SupportMessages_CustomerSupports_CustomerSupportSupportID",
                        column: x => x.CustomerSupportSupportID,
                        principalTable: "CustomerSupports",
                        principalColumn: "SupportID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupports_UserAccountsUserID",
                table: "CustomerSupports",
                column: "UserAccountsUserID");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_CustomerSupportSupportID",
                table: "SupportMessages",
                column: "CustomerSupportSupportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportMessages");

            migrationBuilder.DropTable(
                name: "CustomerSupports");
        }
    }
}
