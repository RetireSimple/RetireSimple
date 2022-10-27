using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Backend.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentType = table.Column<string>(type: "TEXT", nullable: false),
                    InvestmentData = table.Column<string>(type: "TEXT", nullable: false),
                    AnalysisType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.InvestmentId);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentModel",
                columns: table => new
                {
                    InvestmentModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    MinModelData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentModel", x => x.InvestmentModelId);
                    table.ForeignKey(
                        name: "FK_InvestmentModel_Investments_InvestmentId",
                        column: x => x.InvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentModel_InvestmentId",
                table: "InvestmentModel",
                column: "InvestmentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentModel");

            migrationBuilder.DropTable(
                name: "Investments");
        }
    }
}
