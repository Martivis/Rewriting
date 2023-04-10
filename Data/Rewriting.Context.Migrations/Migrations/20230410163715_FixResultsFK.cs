using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class FixResultsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_results_contracts_ContractUid",
                schema: "rewritingdb",
                table: "results");

            migrationBuilder.AddForeignKey(
                name: "FK_results_contracts_ContractUid",
                schema: "rewritingdb",
                table: "results",
                column: "ContractUid",
                principalSchema: "rewritingdb",
                principalTable: "contracts",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_results_contracts_ContractUid",
                schema: "rewritingdb",
                table: "results");

            migrationBuilder.AddForeignKey(
                name: "FK_results_contracts_ContractUid",
                schema: "rewritingdb",
                table: "results",
                column: "ContractUid",
                principalSchema: "rewritingdb",
                principalTable: "contracts",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
