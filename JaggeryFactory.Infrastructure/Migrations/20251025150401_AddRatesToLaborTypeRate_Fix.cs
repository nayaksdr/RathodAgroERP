using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatesToLaborTypeRate_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PerTonRate",
                table: "LaborTypeRates",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerProductionRate",
                table: "LaborTypeRates",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "PerTonRate", table: "LaborTypeRates");
            migrationBuilder.DropColumn(name: "PerProductionRate", table: "LaborTypeRates");
        }
    }
}
