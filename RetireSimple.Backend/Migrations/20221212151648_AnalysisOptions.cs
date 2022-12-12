using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Backend.Migrations
{
    public partial class AnalysisOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Portfolio_PortfolioId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_Profiles_ProfileId",
                table: "Portfolio");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentTransfers_PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PortfolioId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Profiles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "PorfolioId",
                table: "InvestmentTransfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnalysisOptionsOverrides",
                table: "Investments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvestmentVehicles",
                columns: table => new
                {
                    InvestmentVehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentVehicleType = table.Column<string>(type: "TEXT", nullable: false),
                    AnalysisOptionsOverrides = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentVehicles", x => x.InvestmentVehicleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_PorfolioId",
                table: "InvestmentTransfers",
                column: "PorfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments",
                column: "InvestmentVehicleBaseInvestmentVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PorfolioId",
                table: "Expenses",
                column: "PorfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Portfolio_PorfolioId",
                table: "Expenses",
                column: "PorfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments",
                column: "InvestmentVehicleBaseInvestmentVehicleId",
                principalTable: "InvestmentVehicles",
                principalColumn: "InvestmentVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PorfolioId",
                table: "InvestmentTransfers",
                column: "PorfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_Profiles_ProfileId",
                table: "Portfolio",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Portfolio_PorfolioId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PorfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_Profiles_ProfileId",
                table: "Portfolio");

            migrationBuilder.DropTable(
                name: "InvestmentVehicles");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentTransfers_PorfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Investments_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PorfolioId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PorfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropColumn(
                name: "AnalysisOptionsOverrides",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments");

            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "Profiles",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "InvestmentTransfers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_PortfolioId",
                table: "InvestmentTransfers",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PortfolioId",
                table: "Expenses",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Portfolio_PortfolioId",
                table: "Expenses",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PortfolioId",
                table: "InvestmentTransfers",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_Profiles_ProfileId",
                table: "Portfolio",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
