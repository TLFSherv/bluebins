using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixforBookingUserProfileCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_booking_user_profile_UserProfileId",
                schema: "booking",
                table: "booking");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                schema: "booking",
                table: "booking",
                newName: "user_profile_id");

            migrationBuilder.RenameIndex(
                name: "IX_booking_UserProfileId",
                schema: "booking",
                table: "booking",
                newName: "IX_booking_user_profile_id");

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

            migrationBuilder.RenameColumn(
                name: "user_profile_id",
                schema: "booking",
                table: "booking",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_booking_user_profile_id",
                schema: "booking",
                table: "booking",
                newName: "IX_booking_UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_booking_user_profile_UserProfileId",
                schema: "booking",
                table: "booking",
                column: "UserProfileId",
                principalSchema: "booking",
                principalTable: "user_profile",
                principalColumn: "id");
        }
    }
}
