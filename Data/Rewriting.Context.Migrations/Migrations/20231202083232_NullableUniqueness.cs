using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class NullableUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Uniqueness",
                schema: "rewritingdb",
                table: "results",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Uniqueness",
                schema: "rewritingdb",
                table: "results",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
