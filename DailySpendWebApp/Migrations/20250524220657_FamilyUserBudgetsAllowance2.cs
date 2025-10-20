using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class FamilyUserBudgetsAllowance2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyUserBudgetsAllowance",
                columns: table => new
                {
                    AllowancePaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentUserID = table.Column<int>(type: "int", nullable: false),
                    FamilyUserID = table.Column<int>(type: "int", nullable: false),
                    ParentBudgetID = table.Column<int>(type: "int", nullable: false),
                    AllowancePaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllowancePaymentAmount = table.Column<double>(type: "float", nullable: false),
                    IsParentAdded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyUserBudgetsAllowance", x => x.AllowancePaymentID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyUserBudgetsAllowance");
        }
    }
}
