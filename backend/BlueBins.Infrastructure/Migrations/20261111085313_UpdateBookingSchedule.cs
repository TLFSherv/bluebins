using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "booking",
                table: "schedule");

            migrationBuilder.AddColumn<int>(
                name: "day_of_month",
                schema: "booking",
                table: "schedule",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "day_of_week",
                schema: "booking",
                table: "schedule",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "schedule_id",
                schema: "booking",
                table: "booking",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateOnly>(
                name: "booking_date",
                schema: "booking",
                table: "booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day_of_month",
                schema: "booking",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "day_of_week",
                schema: "booking",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "booking_date",
                schema: "booking",
                table: "booking");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                schema: "booking",
                table: "schedule",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<int>(
                name: "schedule_id",
                schema: "booking",
                table: "booking",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
