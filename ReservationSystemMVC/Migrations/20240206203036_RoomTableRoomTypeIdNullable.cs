using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class RoomTableRoomTypeIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "RoomTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_RoomType_RoomTypeId",
                table: "Room",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "RoomTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
