using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class SharedSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "SharedSequence",
                schema: "dbo",
                startValue: 70L);

            migrationBuilder.AddColumn<int>(
                name: "UniqueUserID",
                table: "UserAccounts",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");

            migrationBuilder.AddColumn<int>(
                name: "UniqueUserID",
                table: "FamilyUserAccount",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "SharedSequence",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "UniqueUserID",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UniqueUserID",
                table: "FamilyUserAccount");
        }
    }
}
