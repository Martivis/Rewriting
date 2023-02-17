using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ExtractResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contracts_orders_Uid",
                schema: "rewritingdb",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_offers_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "offers");

            migrationBuilder.DropColumn(
                name: "Result",
                schema: "rewritingdb",
                table: "contracts");

            migrationBuilder.CreateTable(
                name: "results",
                schema: "rewritingdb",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ContractUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_results", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_results_contracts_ContractUid",
                        column: x => x.ContractUid,
                        principalSchema: "rewritingdb",
                        principalTable: "contracts",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_results_ContractUid",
                schema: "rewritingdb",
                table: "results",
                column: "ContractUid");

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_orders_Uid",
                schema: "rewritingdb",
                table: "contracts",
                column: "Uid",
                principalSchema: "rewritingdb",
                principalTable: "orders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_offers_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "offers",
                column: "ContractorUid",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contracts_orders_Uid",
                schema: "rewritingdb",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_offers_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "offers");

            migrationBuilder.DropTable(
                name: "results",
                schema: "rewritingdb");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                schema: "rewritingdb",
                table: "contracts",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_orders_Uid",
                schema: "rewritingdb",
                table: "contracts",
                column: "Uid",
                principalSchema: "rewritingdb",
                principalTable: "orders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_offers_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "offers",
                column: "ContractorUid",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
