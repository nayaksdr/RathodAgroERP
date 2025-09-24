using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchDealer",
                table: "JaggerySales");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Members",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SplitwisePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromMemberId = table.Column<int>(type: "int", nullable: false),
                    ToMemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitwisePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SplitwisePayments_Members_FromMemberId",
                        column: x => x.FromMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SplitwisePayments_Members_PaidById",
                        column: x => x.PaidById,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SplitwisePayments_Members_ToMemberId",
                        column: x => x.ToMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SplitwisePayments_FromMemberId",
                table: "SplitwisePayments",
                column: "FromMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitwisePayments_PaidById",
                table: "SplitwisePayments",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_SplitwisePayments_ToMemberId",
                table: "SplitwisePayments",
                column: "ToMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SplitwisePayments");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "SearchDealer",
                table: "JaggerySales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
