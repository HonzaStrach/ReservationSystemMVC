using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystemMVC.Migrations
{
    /// <inheritdoc />
    public partial class SeasonRoomIdNotRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Room_RoomId",
                table: "Seasons");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Seasons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "RoomRate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "SeasonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Room_RoomId",
                table: "Seasons",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Room_RoomId",
                table: "Seasons");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Seasons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "RoomRate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomRate_Seasons_SeasonId",
                table: "RoomRate",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Room_RoomId",
                table: "Seasons",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
