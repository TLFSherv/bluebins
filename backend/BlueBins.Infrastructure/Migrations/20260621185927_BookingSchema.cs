using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class BookingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "booking");

            migrationBuilder.CreateTable(
                name: "location",
                schema: "booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    maps_id = table.Column<string>(type: "text", nullable: true),
                    address_line_1 = table.Column<string>(type: "text", nullable: false),
                    postcode = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("location_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                schema: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    frequency = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedule_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                schema: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    default_location_id = table.Column<int>(type: "integer", nullable: true),
                    default_schedule_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_profile_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_profile_location_default_location_id",
                        column: x => x.default_location_id,
                        principalSchema: "booking",
                        principalTable: "location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_user_profile_schedule_default_schedule_id",
                        column: x => x.default_schedule_id,
                        principalSchema: "booking",
                        principalTable: "schedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                schema: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_profile_id = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: false),
                    schedule_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    collection_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    date_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("booking_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_location_location_id",
                        column: x => x.location_id,
                        principalSchema: "booking",
                        principalTable: "location",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_booking_schedule_schedule_id",
                        column: x => x.schedule_id,
                        principalSchema: "booking",
                        principalTable: "schedule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_booking_user_profile_user_profile_id",
                        column: x => x.user_profile_id,
                        principalSchema: "booking",
                        principalTable: "user_profile",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "recycling_item",
                schema: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    material_type = table.Column<int>(type: "integer", nullable: true),
                    weight_kg = table.Column<decimal>(type: "numeric", nullable: true),
                    volume_litres = table.Column<decimal>(type: "numeric", nullable: true),
                    contamination_percent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("recycling_item_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_recycling_item_booking_booking_id",
                        column: x => x.booking_id,
                        principalSchema: "booking",
                        principalTable: "booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_booking_location_id",
                schema: "booking",
                table: "booking",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_schedule_id",
                schema: "booking",
                table: "booking",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_user_profile_id",
                schema: "booking",
                table: "booking",
                column: "user_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_location_postcode",
                schema: "booking",
                table: "location",
                column: "postcode");

            migrationBuilder.CreateIndex(
                name: "IX_recycling_item_booking_id",
                schema: "booking",
                table: "recycling_item",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_profile_default_location_id",
                schema: "booking",
                table: "user_profile",
                column: "default_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_profile_default_schedule_id",
                schema: "booking",
                table: "user_profile",
                column: "default_schedule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recycling_item",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "booking",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "user_profile",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "location",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "schedule",
                schema: "booking");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "booking",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "booking",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "booking",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "booking",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "booking",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "booking",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "booking",
                newName: "AspNetRoleClaims");
        }
    }
}
