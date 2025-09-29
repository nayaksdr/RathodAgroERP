using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateJaggerySalePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JaggerySalePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                              .Annotation("SqlServer:Identity", "1, 1"),
                    JaggerySaleId = table.Column<int>(type: "int", nullable: false),
                    FromMemberId = table.Column<int>(type: "int", nullable: false),
                    ToMemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JaggerySalePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_JaggerySales_JaggerySaleId",
                        column: x => x.JaggerySaleId,
                        principalTable: "JaggerySales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_Members_FromMemberId",
                        column: x => x.FromMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_Members_ToMemberId",
                        column: x => x.ToMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JaggerySalePayments_JaggerySaleId",
                table: "JaggerySalePayments",
                column: "JaggerySaleId");

            migrationBuilder.CreateIndex(
                name: "IX_JaggerySalePayments_FromMemberId",
                table: "JaggerySalePayments",
                column: "FromMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_JaggerySalePayments_ToMemberId",
                table: "JaggerySalePayments",
                column: "ToMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JaggerySalePayments");
        }
    }
}
