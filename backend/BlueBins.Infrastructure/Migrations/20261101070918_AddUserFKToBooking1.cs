using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFKToBooking1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_booking_user_profile_id",
                schema: "booking",
                table: "booking",
                column: "user_profile_id");

            migrationBuilder.AddForeignKey(
                name: "FK_booking_user_profile_user_profile_id",
                schema: "booking",
                table: "booking",
                column: "user_profile_id",
                principalSchema: "booking",
                principalTable: "user_profile",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_booking_user_profile_user_profile_id",
                schema: "booking",
                table: "booking");

            migrationBuilder.DropIndex(
                name: "IX_booking_user_profile_id",
                schema: "booking",
                table: "booking");
        }
    }
}
