using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class RoomRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomRate",
                columns: table => new
                {
                    RoomRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NightRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinNights = table.Column<int>(type: "int", nullable: false),
                    ExtraBedRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RebateForLengthOfStay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRate", x => x.RoomRateId);
                    table.ForeignKey(
                        name: "FK_RoomRate_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRate_RoomId",
                table: "RoomRate",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRate");
        }
    }
}
