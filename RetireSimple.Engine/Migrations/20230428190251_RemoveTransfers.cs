using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Engine.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentTransfers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentTransfers",
                columns: table => new
                {
                    InvestmentTransferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DestinationInvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceInvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false, defaultValue: 0.0),
                    TransferDate = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTransfers", x => x.InvestmentTransferId);
                    table.ForeignKey(
                        name: "FK_InvestmentTransfers_Investments_DestinationInvestmentId",
                        column: x => x.DestinationInvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentTransfers_Investments_SourceInvestmentId",
                        column: x => x.SourceInvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_DestinationInvestmentId",
                table: "InvestmentTransfers",
                column: "DestinationInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_SourceInvestmentId",
                table: "InvestmentTransfers",
                column: "SourceInvestmentId");
        }
    }
}
