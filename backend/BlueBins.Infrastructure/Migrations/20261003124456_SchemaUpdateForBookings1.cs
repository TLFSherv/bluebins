using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SchemaUpdateForBookings1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item",
                column: "RecyclingId",
                principalSchema: "public",
                principalTable: "Recyclings",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item",
                column: "RecyclingId",
                principalSchema: "public",
                principalTable: "Recyclings",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
