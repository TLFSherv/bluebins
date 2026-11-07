using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MoveRecylingTableToBookingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropForeignKey(
                name: "FK_Recyclings_booking_booking_id",
                schema: "public",
                table: "Recyclings");

            migrationBuilder.RenameTable(
                name: "Recyclings",
                schema: "public",
                newName: "recycling",
                newSchema: "booking");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_booking_booking_id",
                schema: "booking",
                table: "recycling",
                column: "booking_id",
                principalSchema: "booking",
                principalTable: "booking",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_recycling_RecyclingId",
                schema: "booking",
                table: "recycling_item",
                column: "RecyclingId",
                principalSchema: "booking",
                principalTable: "recycling",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_booking_booking_id",
                schema: "booking",
                table: "recycling");

            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_recycling_RecyclingId",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.RenameTable(
                name: "recycling",
                schema: "booking",
                newName: "Recyclings",
                newSchema: "public");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item",
                column: "RecyclingId",
                principalSchema: "public",
                principalTable: "Recyclings",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recyclings_booking_booking_id",
                schema: "public",
                table: "Recyclings",
                column: "booking_id",
                principalSchema: "booking",
                principalTable: "booking",
                principalColumn: "id");
        }
    }
}
