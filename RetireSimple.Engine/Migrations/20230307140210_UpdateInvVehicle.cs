using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetireSimple.Engine.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashHoldings",
                table: "InvestmentVehicle");

            migrationBuilder.AddColumn<string>(
                name: "InvestmentVehicleData",
                table: "InvestmentVehicle",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "InvestmentVehicle",
                type: "datetime2(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentVehicleData",
                table: "InvestmentVehicle");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "InvestmentVehicle");

            migrationBuilder.AddColumn<decimal>(
                name: "CashHoldings",
                table: "InvestmentVehicle",
                type: "TEXT",
                nullable: false,
                defaultValue: 0.0m);
        }
    }
}
