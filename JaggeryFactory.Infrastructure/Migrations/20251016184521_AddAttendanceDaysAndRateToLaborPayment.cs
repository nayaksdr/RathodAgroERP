using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceDaysAndRateToLaborPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceDays",
                table: "LaborPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rate",
                table: "LaborPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceDays",
                table: "LaborPayments");

            migrationBuilder.DropColumn(
                name: "rate",
                table: "LaborPayments");
        }
    }
}
