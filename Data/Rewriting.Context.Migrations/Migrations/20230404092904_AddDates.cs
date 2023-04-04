using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                schema: "rewritingdb",
                table: "orders",
                newName: "PublishDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "RegistrationDate",
                schema: "rewritingdb",
                table: "users_data",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "offers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "contracts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                schema: "rewritingdb",
                table: "users_data");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "results");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "contracts");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                schema: "rewritingdb",
                table: "orders",
                newName: "DateTime");
        }
    }
}
