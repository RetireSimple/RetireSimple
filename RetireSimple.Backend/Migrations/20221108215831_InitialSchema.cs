using System;
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
                    AnalysisType = table.Column<string>(type: "TEXT", nullable: true),
                    LastAnalysis = table.Column<DateTime>(type: "datetime2(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.InvestmentId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceInvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false, defaultValue: 0.0),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Frequency = table.Column<int>(type: "INTEGER", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Investments_SourceInvestmentId",
                        column: x => x.SourceInvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentModel",
                columns: table => new
                {
                    InvestmentModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    MinModelData = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentModel", x => new { x.InvestmentModelId, x.InvestmentId });
                    table.ForeignKey(
                        name: "FK_InvestmentModel_Investments_InvestmentId",
                        column: x => x.InvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTransfers",
                columns: table => new
                {
                    InvestmentTransferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceInvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    DestinationInvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentTransfers_Investments_SourceInvestmentId",
                        column: x => x.SourceInvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new {
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false).Annotation("Sqlite:Autoincrement", true),
                    UserAge = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStatus = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    UserName = table.Column<string>(type: "STRING", nullable: false)
				},

                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.ProfileId);
				});

            migrationBuilder.CreateTable(
                name: "UserPortfolio",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
				},

                constraints: table =>
                {
					table.PrimaryKey("PK_UserPortfolio", x => x.PortfolioId);
					table.ForeignKey(
						name: "FK_UserPortfolio_UserProfile_UserId",
						column: x => x.UserId,
						principalTable: "UserProfile",
						principalColumn: "ProfileId",
						onDelete: ReferentialAction.Cascade);
				});

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SourceInvestmentId",
                table: "Expenses",
                column: "SourceInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentModel_InvestmentId",
                table: "InvestmentModel",
                column: "InvestmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_DestinationInvestmentId",
                table: "InvestmentTransfers",
                column: "DestinationInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_SourceInvestmentId",
                table: "InvestmentTransfers",
                column: "SourceInvestmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "InvestmentModel");

            migrationBuilder.DropTable(
                name: "InvestmentTransfers");

            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserPortfolio");
        }
    }
}
