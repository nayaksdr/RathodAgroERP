using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaborPaymentFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old foreign key if it exists
            migrationBuilder.DropForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases");

            // Drop LaborPayment FK and index (no structural change needed)
            migrationBuilder.DropForeignKey(
                name: "FK_LaborPayments_Labors_LaborId",
                table: "LaborPayments");

            migrationBuilder.DropIndex(
                name: "IX_LaborPayments_LaborId",
                table: "LaborPayments");

            // ✅ Fix rate column type change (int → decimal)
            migrationBuilder.AlterColumn<decimal>(
                name: "rate",
                table: "LaborPayments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // ✅ Make existing LaborId column nullable in CanePurchases
            migrationBuilder.AlterColumn<int>(
                name: "LaborId",
                table: "CanePurchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // ✅ Limit FarmerName length and make nullable
            migrationBuilder.AlterColumn<string>(
                name: "FarmerName",
                table: "CanePurchases",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // ✅ Re-add foreign key correctly
            migrationBuilder.AddForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop FK again
            migrationBuilder.DropForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases");

            // Revert rate column type
            migrationBuilder.AlterColumn<int>(
                name: "rate",
                table: "LaborPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            // Make LaborId non-nullable again
            migrationBuilder.AlterColumn<int>(
                name: "LaborId",
                table: "CanePurchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Revert FarmerName to unlimited nvarchar
            migrationBuilder.AlterColumn<string>(
                name: "FarmerName",
                table: "CanePurchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            // Recreate index and foreign keys as before
            migrationBuilder.CreateIndex(
                name: "IX_LaborPayments_LaborId",
                table: "LaborPayments",
                column: "LaborId");

            migrationBuilder.AddForeignKey(
                name: "FK_CanePurchases_Labors_LaborId",
                table: "CanePurchases",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaborPayments_Labors_LaborId",
                table: "LaborPayments",
                column: "LaborId",
                principalTable: "Labors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
