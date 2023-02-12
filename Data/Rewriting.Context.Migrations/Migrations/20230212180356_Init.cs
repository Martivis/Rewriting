using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewriting.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rewritingdb");

            migrationBuilder.CreateTable(
                name: "users_data",
                schema: "rewritingdb",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OrdersCount = table.Column<long>(type: "bigint", nullable: false),
                    CompletedContractsCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_data", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "rewritingdb",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientUid = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractorUid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_order_users_data_ClientUid",
                        column: x => x.ClientUid,
                        principalSchema: "rewritingdb",
                        principalTable: "users_data",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_users_data_ContractorUid",
                        column: x => x.ContractorUid,
                        principalSchema: "rewritingdb",
                        principalTable: "users_data",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "offers",
                schema: "rewritingdb",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractorUid = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offers", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_offers_order_OrderUid",
                        column: x => x.OrderUid,
                        principalSchema: "rewritingdb",
                        principalTable: "order",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_offers_users_data_ContractorUid",
                        column: x => x.ContractorUid,
                        principalSchema: "rewritingdb",
                        principalTable: "users_data",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_offers_ContractorUid",
                schema: "rewritingdb",
                table: "offers",
                column: "ContractorUid");

            migrationBuilder.CreateIndex(
                name: "IX_offers_OrderUid",
                schema: "rewritingdb",
                table: "offers",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_order_ClientUid",
                schema: "rewritingdb",
                table: "order",
                column: "ClientUid");

            migrationBuilder.CreateIndex(
                name: "IX_order_ContractorUid",
                schema: "rewritingdb",
                table: "order",
                column: "ContractorUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offers",
                schema: "rewritingdb");

            migrationBuilder.DropTable(
                name: "order",
                schema: "rewritingdb");

            migrationBuilder.DropTable(
                name: "users_data",
                schema: "rewritingdb");
        }
    }
}
