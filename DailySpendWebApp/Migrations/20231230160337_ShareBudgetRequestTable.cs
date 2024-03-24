using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class ShareBudgetRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShareBudgetRequest",
                columns: table => new
                {
                    SharedBudgetRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SharedBudgetID = table.Column<int>(type: "int", nullable: false),
                    SharedWithUserAccountID = table.Column<int>(type: "int", nullable: false),
                    SharedWithUserEmail = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    SharedToUserEmail = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareBudgetRequest", x => x.SharedBudgetRequestID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareBudgetRequest");
        }
    }
}
