using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormStatus",
                schema: "dbo",
                table: "FormStructures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UrlIdentifier",
                schema: "dbo",
                table: "FormStructures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlIdentifier",
                schema: "dbo",
                table: "FormPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormStatus",
                schema: "dbo",
                table: "FormStructures");

            migrationBuilder.DropColumn(
                name: "UrlIdentifier",
                schema: "dbo",
                table: "FormStructures");

            migrationBuilder.DropColumn(
                name: "UrlIdentifier",
                schema: "dbo",
                table: "FormPages");
        }
    }
}
