using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.API.Migrations
{
    /// <inheritdoc />
    public partial class seededcountriesandmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Nigeria", "NGA" },
                    { 2, "Mamlaka Saudiyya", "KSA" },
                    { 3, "United Arab Emirate", "UAE" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "Plot 445,Ozumba Mbadiwe,Victoria Island, Lagos.", 1, "Oriental Hotel", 4.0 },
                    { 2, "6623 Al Shohadaa Eastern Ring Road, Riyadh.", 2, "Hilton Riyadh Hotel", 5.0 },
                    { 3, "Al Mustaqbal St - opposite The Dubai Mall - Trade Centre - Dubai", 3, "Swissôtel Al Murooj Dubai", 5.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: 3);
        }
    }
}
