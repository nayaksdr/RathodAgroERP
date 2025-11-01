using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaborPaymentAndAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {          

          

         

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JaggerySales_Labors_LaborId",
                table: "JaggerySales");

            migrationBuilder.DropIndex(
                name: "IX_JaggerySales_LaborId",
                table: "JaggerySales");

            migrationBuilder.DropColumn(
                name: "LaborTypeName",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "LaborId",
                table: "JaggerySales");
        }
    }
}
