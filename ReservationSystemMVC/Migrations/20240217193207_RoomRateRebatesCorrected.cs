using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class RoomRateRebatesCorrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRateRoomRateRebate");

            migrationBuilder.CreateTable(
                name: "RoomRateRoomRateRebates",
                columns: table => new
                {
                    RoomRateId = table.Column<int>(type: "int", nullable: false),
                    RoomRateRebateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRateRoomRateRebates", x => new { x.RoomRateId, x.RoomRateRebateId });
                    table.ForeignKey(
                        name: "FK_RoomRateRoomRateRebates_RoomRateRebate_RoomRateRebateId",
                        column: x => x.RoomRateRebateId,
                        principalTable: "RoomRateRebate",
                        principalColumn: "RoomRateRebateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRateRoomRateRebates_RoomRate_RoomRateId",
                        column: x => x.RoomRateId,
                        principalTable: "RoomRate",
                        principalColumn: "RoomRateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRateRoomRateRebates_RoomRateRebateId",
                table: "RoomRateRoomRateRebates",
                column: "RoomRateRebateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRateRoomRateRebates");

            migrationBuilder.CreateTable(
                name: "RoomRateRoomRateRebate",
                columns: table => new
                {
                    RoomRateId = table.Column<int>(type: "int", nullable: false),
                    RoomRateRebateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRateRoomRateRebate", x => new { x.RoomRateId, x.RoomRateRebateId });
                    table.ForeignKey(
                        name: "FK_RoomRateRoomRateRebate_RoomRateRebate_RoomRateRebateId",
                        column: x => x.RoomRateRebateId,
                        principalTable: "RoomRateRebate",
                        principalColumn: "RoomRateRebateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRateRoomRateRebate_RoomRate_RoomRateId",
                        column: x => x.RoomRateId,
                        principalTable: "RoomRate",
                        principalColumn: "RoomRateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRateRoomRateRebate_RoomRateRebateId",
                table: "RoomRateRoomRateRebate",
                column: "RoomRateRebateId");
        }
    }
}
