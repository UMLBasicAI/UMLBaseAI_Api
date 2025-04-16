using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Base.DataBaseAndIdentity.Migrations.M_AppDbContext
{
    /// <inheritdoc />
    public partial class M2_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "content",
                schema: "uml_base_ai",
                table: "message",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "action",
                schema: "uml_base_ai",
                table: "history",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "content",
                schema: "uml_base_ai",
                table: "message",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "action",
                schema: "uml_base_ai",
                table: "history",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
