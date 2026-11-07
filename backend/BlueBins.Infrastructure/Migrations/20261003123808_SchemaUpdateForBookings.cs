using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SchemaUpdateForBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_booking_booking_id",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropPrimaryKey(
                name: "recycling_item_pkey",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropIndex(
                name: "IX_recycling_item_booking_id",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "booking",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropColumn(
                name: "address_line_1",
                schema: "booking",
                table: "location");

            migrationBuilder.DropColumn(
                name: "collection_date",
                schema: "booking",
                table: "booking");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "booking",
                table: "schedule",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_date",
                schema: "booking",
                table: "schedule",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "booking_id",
                schema: "booking",
                table: "recycling_item",
                newName: "RecyclingId");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "booking",
                table: "user_profile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<int>(
                name: "frequency",
                schema: "booking",
                table: "schedule",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "postcode",
                schema: "booking",
                table: "location",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "maps_id",
                schema: "booking",
                table: "location",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                schema: "booking",
                table: "location",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "parish",
                schema: "booking",
                table: "location",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_profile_id",
                schema: "booking",
                table: "booking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldNullable: true);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_recycling_item",
                schema: "booking",
                table: "recycling_item",
                columns: new[] { "RecyclingId", "material_type" });

            migrationBuilder.CreateTable(
                name: "Recyclings",
                schema: "public",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    NumberOfBags = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("recycling_item_pkey", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Recyclings_booking_booking_id",
                        column: x => x.booking_id,
                        principalSchema: "booking",
                        principalTable: "booking",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_location_address",
                schema: "booking",
                table: "location",
                column: "address");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recycling_item_Recyclings_RecyclingId",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropTable(
                name: "Recyclings",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recycling_item",
                schema: "booking",
                table: "recycling_item");

            migrationBuilder.DropIndex(
                name: "IX_location_address",
                schema: "booking",
                table: "location");

            migrationBuilder.DropColumn(
                name: "address",
                schema: "booking",
                table: "location");

            migrationBuilder.DropColumn(
                name: "parish",
                schema: "booking",
                table: "location");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "booking",
                table: "schedule",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "booking",
                table: "schedule",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "RecyclingId",
                schema: "booking",
                table: "recycling_item",
                newName: "booking_id");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "booking",
                table: "user_profile",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "frequency",
                schema: "booking",
                table: "schedule",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "booking",
                table: "schedule",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "booking",
                table: "recycling_item",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "postcode",
                schema: "booking",
                table: "location",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "maps_id",
                schema: "booking",
                table: "location",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address_line_1",
                schema: "booking",
                table: "location",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_profile_id",
                schema: "booking",
                table: "booking",
                type: "character varying(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "schedule_id",
                schema: "booking",
                table: "booking",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "collection_date",
                schema: "booking",
                table: "booking",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "recycling_item_pkey",
                schema: "booking",
                table: "recycling_item",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_recycling_item_booking_id",
                schema: "booking",
                table: "recycling_item",
                column: "booking_id");

            migrationBuilder.AddForeignKey(
                name: "FK_recycling_item_booking_booking_id",
                schema: "booking",
                table: "recycling_item",
                column: "booking_id",
                principalSchema: "booking",
                principalTable: "booking",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
