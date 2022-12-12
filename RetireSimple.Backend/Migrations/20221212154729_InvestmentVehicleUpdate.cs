using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Backend.Migrations
{
    public partial class InvestmentVehicleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "InvestmentVehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentVehicles_PortfolioId",
                table: "InvestmentVehicles",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments",
                column: "InvestmentVehicleBaseInvestmentVehicleId",
                principalTable: "InvestmentVehicles",
                principalColumn: "InvestmentVehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentVehicles_Portfolio_PortfolioId",
                table: "InvestmentVehicles",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentVehicles_Portfolio_PortfolioId",
                table: "InvestmentVehicles");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentVehicles_PortfolioId",
                table: "InvestmentVehicles");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "InvestmentVehicles");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_InvestmentVehicles_InvestmentVehicleBaseInvestmentVehicleId",
                table: "Investments",
                column: "InvestmentVehicleBaseInvestmentVehicleId",
                principalTable: "InvestmentVehicles",
                principalColumn: "InvestmentVehicleId");
        }
    }
}
