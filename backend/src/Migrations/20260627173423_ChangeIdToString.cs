using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // STEP 1: Drop the existing Foreign Key constraint so the columns are unlocked
            migrationBuilder.Sql("ALTER TABLE booking.booking DROP CONSTRAINT IF EXISTS \"FK_booking_user_profile_user_profile_id\";");

            // STEP 2: Drop the Identity sequence from the primary key column while it's still an integer
            migrationBuilder.Sql("ALTER TABLE booking.user_profile ALTER COLUMN id DROP IDENTITY IF EXISTS;");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "booking",
                table: "user_profile",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "user_profile_id",
                schema: "booking",
                table: "booking",
                type: "character varying(80)",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "weight_kg",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "volume_litres",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "material_type",
                schema: "booking",
                table: "recycling_item",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "contamination_percent",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_modified",
                schema: "booking",
                table: "booking",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "booking",
                table: "user_profile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "weight_kg",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "volume_litres",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "material_type",
                schema: "booking",
                table: "recycling_item",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "contamination_percent",
                schema: "booking",
                table: "recycling_item",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "user_profile_id",
                schema: "booking",
                table: "booking",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_modified",
                schema: "booking",
                table: "booking",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "now()");
        }
    }
}
