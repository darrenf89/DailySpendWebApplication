using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    public partial class removenull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UniqueUserID",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");

            migrationBuilder.AlterColumn<int>(
                name: "UniqueUserID",
                table: "FamilyUserAccount",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UniqueUserID",
                table: "UserAccounts",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");

            migrationBuilder.AlterColumn<int>(
                name: "UniqueUserID",
                table: "FamilyUserAccount",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR dbo.SharedSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR dbo.SharedSequence");
        }
    }
}
