using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJaggerySaleShareExpensePaymentDistributeTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JaggerySaleShareExpensePayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JaggerySaleShareId = table.Column<int>(type: "int", nullable: false),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JaggerySaleShareExpensePayments", x => x.Id);
                    // Add foreign keys if necessary
                    // table.ForeignKey("FK_JaggerySaleShareExpensePayments_JaggerySaleShares", x => x.JaggerySaleShareId, "JaggerySaleShares", "Id", onDelete: ReferentialAction.Cascade);
                    // table.ForeignKey("FK_JaggerySaleShareExpensePayments_Expenses", x => x.ExpenseId, "Expenses", "Id", onDelete: ReferentialAction.Cascade);
                });

            // Optionally create indexes
            // migrationBuilder.CreateIndex(
            //     name: "IX_JaggerySaleShareExpensePayments_JaggerySaleShareId",
            //     table: "JaggerySaleShareExpensePayments",
            //     column: "JaggerySaleShareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JaggerySaleShareExpensePayments");
        }
    }
}

