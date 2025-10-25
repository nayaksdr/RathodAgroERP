using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCaneProcessingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "LaborTypeRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PerProductionRate",
                table: "LaborTypeRates",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerTonRate",
                table: "LaborTypeRates",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerTon",
                table: "LaborPayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerUnit",
                table: "LaborPayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalJaggeryQty",
                table: "LaborPayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTons",
                table: "LaborPayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaneProcessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalTons = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaneProcessings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaneProcessings_Labors_LaborId",
                        column: x => x.LaborId,
                        principalTable: "Labors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JaggeryProductions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JaggeryProductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JaggeryProductions_Labors_LaborId",
                        column: x => x.LaborId,
                        principalTable: "Labors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaborPayments_LaborId",
                table: "LaborPayments",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_CaneProcessings_LaborId",
                table: "CaneProcessings",
                column: "LaborId");

            migrationBuilder.CreateIndex(
                name: "IX_JaggeryProductions_LaborId",
                table: "JaggeryProductions",
                column: "LaborId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaborPayments_Labors_LaborId",
                table: "LaborPayments",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaborPayments_Labors_LaborId",
                table: "LaborPayments");

            migrationBuilder.DropTable(
                name: "CaneProcessings");

            migrationBuilder.DropTable(
                name: "JaggeryProductions");

            migrationBuilder.DropIndex(
                name: "IX_LaborPayments_LaborId",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "LaborTypeRates");

            migrationBuilder.DropColumn(
                name: "PerProductionRate",
                table: "LaborTypeRates");

            migrationBuilder.DropColumn(
                name: "PerTonRate",
                table: "LaborTypeRates");

            migrationBuilder.DropColumn(
                name: "RatePerTon",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "RatePerUnit",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "TotalJaggeryQty",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "TotalTons",
                table: "LaborPayments");
        }
    }
}
