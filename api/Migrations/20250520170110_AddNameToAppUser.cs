using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaiVe.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "038ede64-4e96-4892-9e79-c4379ad87e60");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b453874-31ee-48cb-863b-15d6d535bfe0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c6cd27f-de8f-44c7-be68-f56c140e889a", null, "User", "USER" },
                    { "dca77fae-4d85-4739-9112-fc048979bc8a", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c6cd27f-de8f-44c7-be68-f56c140e889a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dca77fae-4d85-4739-9112-fc048979bc8a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "038ede64-4e96-4892-9e79-c4379ad87e60", null, "User", "USER" },
                    { "8b453874-31ee-48cb-863b-15d6d535bfe0", null, "Admin", "ADMIN" }
                });
        }
    }
}
