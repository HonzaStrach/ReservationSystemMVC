using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class Season : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomRoomEquipment_RoomEquipment_RoomEquipmentId",
                table: "RoomRoomEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRoomEquipment_Room_RoomId",
                table: "RoomRoomEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomRoomEquipment",
                table: "RoomRoomEquipment");

            migrationBuilder.RenameTable(
                name: "RoomRoomEquipment",
                newName: "RoomRoomEquipments");

            migrationBuilder.RenameIndex(
                name: "IX_RoomRoomEquipment_RoomEquipmentId",
                table: "RoomRoomEquipments",
                newName: "IX_RoomRoomEquipments_RoomEquipmentId");

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "RoomRate",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomRoomEquipments",
                table: "RoomRoomEquipments",
                columns: new[] { "RoomId", "RoomEquipmentId" });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.SeasonId);
                    table.ForeignKey(
                        name: "FK_Seasons_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonDates",
                columns: table => new
                {
                    SeasonDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateApplied = table.Column<DateTime>(type: "date", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonDates", x => x.SeasonDateId);
                    table.ForeignKey(
                        name: "FK_SeasonDates_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRate_SeasonId",
                table: "RoomRate",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonDates_SeasonId",
                table: "SeasonDates",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_RoomId",
                table: "Seasons",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRoomEquipments_RoomEquipment_RoomEquipmentId",
                table: "RoomRoomEquipments",
                column: "RoomEquipmentId",
                principalTable: "RoomEquipment",
                principalColumn: "RoomEquipmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRoomEquipments_Room_RoomId",
                table: "RoomRoomEquipments",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRoomEquipments_RoomEquipment_RoomEquipmentId",
                table: "RoomRoomEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomRoomEquipments_Room_RoomId",
                table: "RoomRoomEquipments");

            migrationBuilder.DropTable(
                name: "SeasonDates");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_RoomRate_SeasonId",
                table: "RoomRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomRoomEquipments",
                table: "RoomRoomEquipments");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "RoomRate");

            migrationBuilder.RenameTable(
                name: "RoomRoomEquipments",
                newName: "RoomRoomEquipment");

            migrationBuilder.RenameIndex(
                name: "IX_RoomRoomEquipments_RoomEquipmentId",
                table: "RoomRoomEquipment",
                newName: "IX_RoomRoomEquipment_RoomEquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomRoomEquipment",
                table: "RoomRoomEquipment",
                columns: new[] { "RoomId", "RoomEquipmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRoomEquipment_RoomEquipment_RoomEquipmentId",
                table: "RoomRoomEquipment",
                column: "RoomEquipmentId",
                principalTable: "RoomEquipment",
                principalColumn: "RoomEquipmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRoomEquipment_Room_RoomId",
                table: "RoomRoomEquipment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
