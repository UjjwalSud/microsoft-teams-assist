using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class FormDesigner_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormStructures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormStructureType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroScript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroductionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKLastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FKDeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormPages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKFormStructurePKId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroScript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKLastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FKDeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormPages_FormStructures_FKFormStructurePKId",
                        column: x => x.FKFormStructurePKId,
                        principalSchema: "dbo",
                        principalTable: "FormStructures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormPageFields",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKFormPagePKId = table.Column<int>(type: "int", nullable: false),
                    FKFormPageFieldPKId = table.Column<int>(type: "int", nullable: true),
                    FormPageFieldType = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LabelExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    DisabledExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HideExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Columns = table.Column<int>(type: "int", nullable: false),
                    RowId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrefixExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuffixExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    ValidationPattern = table.Column<int>(type: "int", nullable: true),
                    MinimumLength = table.Column<int>(type: "int", nullable: true),
                    MinimumExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumLength = table.Column<int>(type: "int", nullable: true),
                    MaximumExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomRegularExpression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberDecimalDigits = table.Column<int>(type: "int", nullable: true),
                    DateTimeType = table.Column<int>(type: "int", nullable: true),
                    SelectDefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUploadValidFileType = table.Column<int>(type: "int", nullable: true),
                    FileUploadCustomExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUploadCustomValidationExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKLastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FKDeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormPageFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormPageFields_FormPageFields_FKFormPageFieldPKId",
                        column: x => x.FKFormPageFieldPKId,
                        principalSchema: "dbo",
                        principalTable: "FormPageFields",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FormPageFields_FormPages_FKFormPagePKId",
                        column: x => x.FKFormPagePKId,
                        principalSchema: "dbo",
                        principalTable: "FormPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormPageFieldOptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKFormPageFieldPKId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKLastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FKDeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormPageFieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormPageFieldOptions_FormPageFields_FKFormPageFieldPKId",
                        column: x => x.FKFormPageFieldPKId,
                        principalSchema: "dbo",
                        principalTable: "FormPageFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormPageFieldOptions_FKFormPageFieldPKId",
                schema: "dbo",
                table: "FormPageFieldOptions",
                column: "FKFormPageFieldPKId");

            migrationBuilder.CreateIndex(
                name: "IX_FormPageFields_FKFormPageFieldPKId",
                schema: "dbo",
                table: "FormPageFields",
                column: "FKFormPageFieldPKId");

            migrationBuilder.CreateIndex(
                name: "IX_FormPageFields_FKFormPagePKId",
                schema: "dbo",
                table: "FormPageFields",
                column: "FKFormPagePKId");

            migrationBuilder.CreateIndex(
                name: "IX_FormPages_FKFormStructurePKId",
                schema: "dbo",
                table: "FormPages",
                column: "FKFormStructurePKId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormPageFieldOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FormPageFields",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FormPages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FormStructures",
                schema: "dbo");
        }
    }
}
