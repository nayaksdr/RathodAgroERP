using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentToCanePurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CanePurchaseId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AadhaarNumber",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "FarmerName",
                table: "CanePurchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CanePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FarmerId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanePayments_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CanePurchaseId",
                table: "Payments",
                column: "CanePurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CanePayments_FarmerId",
                table: "CanePayments",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CanePurchases_CanePurchaseId",
                table: "Payments",
                column: "CanePurchaseId",
                principalTable: "CanePurchases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CanePurchases_CanePurchaseId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CanePayments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CanePurchaseId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CanePurchaseId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FarmerName",
                table: "CanePurchases");

            migrationBuilder.AlterColumn<string>(
                name: "AadhaarNumber",
                table: "Farmers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
