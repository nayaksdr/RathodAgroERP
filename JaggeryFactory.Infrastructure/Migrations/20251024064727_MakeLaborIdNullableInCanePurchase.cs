using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeLaborIdNullableInCanePurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Drop existing FK first (if it exists)
            migrationBuilder.DropForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases");

            // 2️⃣ Alter existing column to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "LaborId",
                table: "CanePurchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // 3️⃣ Recreate FK (optional: use Restrict or SetNull instead of Cascade)
            migrationBuilder.AddForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the changes if needed

            migrationBuilder.DropForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases");

            migrationBuilder.AlterColumn<int>(
                name: "LaborId",
                table: "CanePurchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
