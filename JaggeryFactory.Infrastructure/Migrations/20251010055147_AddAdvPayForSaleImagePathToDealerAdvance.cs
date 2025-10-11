using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvPayForSaleImagePathToDealerAdvance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaidById",
                table: "DealerAdvances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "DealerAdvances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProofImage",
                table: "DealerAdvances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerAdvances_PaidById",
                table: "DealerAdvances",
                column: "PaidById");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAdvances_Members_PaidById",
                table: "DealerAdvances",
                column: "PaidById",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerAdvances_Members_PaidById",
                table: "DealerAdvances");

            migrationBuilder.DropIndex(
                name: "IX_DealerAdvances_PaidById",
                table: "DealerAdvances");

            migrationBuilder.DropColumn(
                name: "PaidById",
                table: "DealerAdvances");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "DealerAdvances");

            migrationBuilder.DropColumn(
                name: "ProofImage",
                table: "DealerAdvances");
        }
    }
}
