using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Reconfigure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_order_OrderUid",
                schema: "rewritingdb",
                table: "offers");

            migrationBuilder.DropForeignKey(
                name: "FK_order_users_data_ClientUid",
                schema: "rewritingdb",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "FK_order_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order",
                schema: "rewritingdb",
                table: "order");

            migrationBuilder.DropIndex(
                name: "IX_order_ContractorUid",
                schema: "rewritingdb",
                table: "order");

            migrationBuilder.DropColumn(
                name: "ContractorUid",
                schema: "rewritingdb",
                table: "order");

            migrationBuilder.RenameTable(
                name: "order",
                schema: "rewritingdb",
                newName: "orders",
                newSchema: "rewritingdb");

            migrationBuilder.RenameIndex(
                name: "IX_order_ClientUid",
                schema: "rewritingdb",
                table: "orders",
                newName: "IX_orders_ClientUid");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "rewritingdb",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                schema: "rewritingdb",
                table: "orders",
                column: "Uid");

            migrationBuilder.CreateTable(
                name: "contracts",
                schema: "rewritingdb",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: true),
                    ContractorUid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_contracts_orders_Uid",
                        column: x => x.Uid,
                        principalSchema: "rewritingdb",
                        principalTable: "orders",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contracts_users_data_ContractorUid",
                        column: x => x.ContractorUid,
                        principalSchema: "rewritingdb",
                        principalTable: "users_data",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contracts_ContractorUid",
                schema: "rewritingdb",
                table: "contracts",
                column: "ContractorUid");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_orders_OrderUid",
                schema: "rewritingdb",
                table: "offers",
                column: "OrderUid",
                principalSchema: "rewritingdb",
                principalTable: "orders",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_data_ClientUid",
                schema: "rewritingdb",
                table: "orders",
                column: "ClientUid",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_offers_orders_OrderUid",
                schema: "rewritingdb",
                table: "offers");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_data_ClientUid",
                schema: "rewritingdb",
                table: "orders");

            migrationBuilder.DropTable(
                name: "contracts",
                schema: "rewritingdb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                schema: "rewritingdb",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "rewritingdb",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                schema: "rewritingdb",
                newName: "order",
                newSchema: "rewritingdb");

            migrationBuilder.RenameIndex(
                name: "IX_orders_ClientUid",
                schema: "rewritingdb",
                table: "order",
                newName: "IX_order_ClientUid");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractorUid",
                schema: "rewritingdb",
                table: "order",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_order",
                schema: "rewritingdb",
                table: "order",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_order_ContractorUid",
                schema: "rewritingdb",
                table: "order",
                column: "ContractorUid");

            migrationBuilder.AddForeignKey(
                name: "FK_offers_order_OrderUid",
                schema: "rewritingdb",
                table: "offers",
                column: "OrderUid",
                principalSchema: "rewritingdb",
                principalTable: "order",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_order_users_data_ClientUid",
                schema: "rewritingdb",
                table: "order",
                column: "ClientUid",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_order_users_data_ContractorUid",
                schema: "rewritingdb",
                table: "order",
                column: "ContractorUid",
                principalSchema: "rewritingdb",
                principalTable: "users_data",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
