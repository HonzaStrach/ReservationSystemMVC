using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class DayRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RebateForLengthOfStay",
                table: "RoomRate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApplied",
                table: "RoomRate",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApplied",
                table: "RoomRate");

            migrationBuilder.AddColumn<decimal>(
                name: "RebateForLengthOfStay",
                table: "RoomRate",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
