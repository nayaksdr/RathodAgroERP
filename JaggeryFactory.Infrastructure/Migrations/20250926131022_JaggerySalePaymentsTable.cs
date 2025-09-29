using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaggeryAgro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JaggerySalePaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JaggerySalePayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JaggerySaleId = table.Column<int>(nullable: false),
                    FromMemberId = table.Column<int>(nullable: false),
                    ToMemberId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentMode = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JaggerySalePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_JaggerySales_JaggerySaleId",
                        column: x => x.JaggerySaleId,
                        principalTable: "JaggerySales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_Members_FromMemberId",
                        column: x => x.FromMemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JaggerySalePayments_Members_ToMemberId",
                        column: x => x.ToMemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JaggerySalePayments");
        }

    }
}
