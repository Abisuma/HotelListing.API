using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class editedDecimaltodecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ac8dadd-ebbd-4650-8e39-b405e07f2d10");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71604124-a363-4993-b683-7c0c33215034");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "BookHotels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b5466471-f874-444e-b7c5-4bf6303f8c13", null, "Admin", "ADMINISTRATOR" },
                    { "d41c3c6e-14e9-4aa8-93e0-5a009d36bd61", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5466471-f874-444e-b7c5-4bf6303f8c13");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d41c3c6e-14e9-4aa8-93e0-5a009d36bd61");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "BookHotels",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ac8dadd-ebbd-4650-8e39-b405e07f2d10", null, "User", "USER" },
                    { "71604124-a363-4993-b683-7c0c33215034", null, "Admin", "ADMINISTRATOR" }
                });
        }
    }
}
