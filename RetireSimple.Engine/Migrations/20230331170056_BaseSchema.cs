﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Engine.Migrations
{
    /// <inheritdoc />
    public partial class BaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ProfileId);
                });

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.PortfolioId);
                    table.ForeignKey(
                        name: "FK_Portfolio_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentVehicle",
                columns: table => new
                {
                    InvestmentVehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestmentVehicleName = table.Column<string>(type: "TEXT", nullable: true),
                    InvestmentVehicleType = table.Column<string>(type: "TEXT", nullable: false),
                    InvestmentVehicleModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    InvestmentVehicleData = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    AnalysisOptionsOverrides = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentVehicle", x => x.InvestmentVehicleId);
                    table.ForeignKey(
                        name: "FK_InvestmentVehicle_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioModel",
                columns: table => new
                {
                    PortfolioModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    MinModelData = table.Column<string>(type: "TEXT", nullable: false),
                    AvgModelData = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioModel", x => x.PortfolioModelId);
                    table.ForeignKey(
                        name: "FK_PortfolioModel_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentName = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    InvestmentType = table.Column<string>(type: "TEXT", nullable: false),
                    InvestmentData = table.Column<string>(type: "TEXT", nullable: false),
                    AnalysisOptionsOverrides = table.Column<string>(type: "TEXT", nullable: false),
                    AnalysisType = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestmentVehicleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.InvestmentId);
                    table.ForeignKey(
                        name: "FK_Investments_InvestmentVehicle_InvestmentVehicleId",
                        column: x => x.InvestmentVehicleId,
                        principalTable: "InvestmentVehicle",
                        principalColumn: "InvestmentVehicleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investments_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentVehicleModel",
                columns: table => new
                {
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentVehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    MinModelData = table.Column<string>(type: "TEXT", nullable: false),
                    AvgModelData = table.Column<string>(type: "TEXT", nullable: false),
                    TaxDeductedMaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    TaxDeductedMinModelData = table.Column<string>(type: "TEXT", nullable: false),
                    TaxDeductedAvgModelData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentVehicleModel", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_InvestmentVehicleModel_InvestmentVehicle_InvestmentVehicleId",
                        column: x => x.InvestmentVehicleId,
                        principalTable: "InvestmentVehicle",
                        principalColumn: "InvestmentVehicleId",
                        onDelete: ReferentialAction.Cascade);
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
                    InvestmentModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxModelData = table.Column<string>(type: "TEXT", nullable: false),
                    MinModelData = table.Column<string>(type: "TEXT", nullable: false),
                    AvgModelData = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentTransfers_Investments_SourceInvestmentId",
                        column: x => x.SourceInvestmentId,
                        principalTable: "Investments",
                        principalColumn: "InvestmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "ProfileId", "Age", "Name", "Status" },
                values: new object[] { 1, 65, "Default", true });

            migrationBuilder.InsertData(
                table: "Portfolio",
                columns: new[] { "PortfolioId", "LastUpdated", "PortfolioName", "ProfileId" },
                values: new object[] { 1, null, "Default", 1 });

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
                name: "IX_Investments_InvestmentVehicleId",
                table: "Investments",
                column: "InvestmentVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_PortfolioId",
                table: "Investments",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_DestinationInvestmentId",
                table: "InvestmentTransfers",
                column: "DestinationInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransfers_SourceInvestmentId",
                table: "InvestmentTransfers",
                column: "SourceInvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentVehicle_PortfolioId",
                table: "InvestmentVehicle",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentVehicleModel_InvestmentVehicleId",
                table: "InvestmentVehicleModel",
                column: "InvestmentVehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_ProfileId",
                table: "Portfolio",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioModel_PortfolioId",
                table: "PortfolioModel",
                column: "PortfolioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "InvestmentModel");

            migrationBuilder.DropTable(
                name: "InvestmentTransfers");

            migrationBuilder.DropTable(
                name: "InvestmentVehicleModel");

            migrationBuilder.DropTable(
                name: "PortfolioModel");

            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "InvestmentVehicle");

            migrationBuilder.DropTable(
                name: "Portfolio");

            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
