using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Backend.Migrations
{
    public partial class portfolios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "InvestmentTransfers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Investments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorfolioId",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                });

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.PortfolioId);
                    table.ForeignKey(
                        name: "FK_Portfolio_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_PortfolioId",
                table: "InvestmentTransfers",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_PortfolioId",
                table: "Investments",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PortfolioId",
                table: "Expenses",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_ProfileId",
                table: "Portfolio",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Portfolio_PortfolioId",
                table: "Expenses",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Portfolio_PortfolioId",
                table: "Investments",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PortfolioId",
                table: "InvestmentTransfers",
                column: "PortfolioId",
                principalTable: "Portfolio",
                principalColumn: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Portfolio_PortfolioId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Portfolio_PortfolioId",
                table: "Investments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentTransfers_Portfolio_PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropTable(
                name: "Portfolio");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentTransfers_PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Investments_PortfolioId",
                table: "Investments");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PortfolioId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "InvestmentTransfers");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "PorfolioId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Expenses");
        }
    }
}
