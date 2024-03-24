using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class firebasedevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FireBaseToken",
                table: "UserAccounts");

            migrationBuilder.CreateTable(
                name: "FirebaseDevices",
                columns: table => new
                {
                    FirebaseDeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirebaseToken = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UserAccountID = table.Column<int>(type: "int", nullable: false),
                    LoginExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirebaseDevices", x => x.FirebaseDeviceID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirebaseDevices");

            migrationBuilder.AddColumn<string>(
                name: "FireBaseToken",
                table: "UserAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
