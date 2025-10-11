using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaidByAndPaymentModeToJaggerySale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaidById",
                table: "JaggerySales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JaggerySales_PaidById",
                table: "JaggerySales",
                column: "PaidById");

            migrationBuilder.AddForeignKey(
                name: "FK_JaggerySales_Members_PaidById",
                table: "JaggerySales",
                column: "PaidById",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JaggerySales_Members_PaidById",
                table: "JaggerySales");

            migrationBuilder.DropIndex(
                name: "IX_JaggerySales_PaidById",
                table: "JaggerySales");

            migrationBuilder.DropColumn(
                name: "PaidById",
                table: "JaggerySales");
        }
    }
}
