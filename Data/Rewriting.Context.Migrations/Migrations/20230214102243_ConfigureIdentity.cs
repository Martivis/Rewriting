using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_users_users_data_Id",
                schema: "rewritingdb",
                table: "users",
                column: "Id",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_users_data_Id",
                schema: "rewritingdb",
                table: "users");
        }
    }
}
