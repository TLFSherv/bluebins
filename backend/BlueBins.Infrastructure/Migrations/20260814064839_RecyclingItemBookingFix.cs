using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RecyclingItemBookingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_booking_BookingId1",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropIndex(
                name: "IX_recycling_item_BookingId1",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                schema: "booking",
                table: "recycling_item");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId1",
                schema: "booking",
                table: "recycling_item",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_recycling_item_BookingId1",
                schema: "booking",
                table: "recycling_item",
                column: "BookingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_booking_BookingId1",
                schema: "booking",
                table: "recycling_item",
                column: "BookingId1",
                principalSchema: "booking",
                principalTable: "booking",
                principalColumn: "id");
        }
    }
}
