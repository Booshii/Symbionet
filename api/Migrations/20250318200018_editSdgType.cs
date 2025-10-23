using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaiVe.Migrations
{
    /// <inheritdoc />
    public partial class editSdgType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "459a0d16-fa74-4de4-b723-8302fc5d5a5e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84758956-3dcd-48b5-9ec7-8f29b52857cc");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SdgTypes",
                newName: "Name");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5325d50a-8a61-43a2-a31a-e7ad1b65c03e", null, "Admin", "ADMIN" },
                    { "f8f8fa2d-4f61-432b-b416-941e40542942", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5325d50a-8a61-43a2-a31a-e7ad1b65c03e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8f8fa2d-4f61-432b-b416-941e40542942");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SdgTypes",
                newName: "Title");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "459a0d16-fa74-4de4-b723-8302fc5d5a5e", null, "User", "USER" },
                    { "84758956-3dcd-48b5-9ec7-8f29b52857cc", null, "Admin", "ADMIN" }
                });
        }
    }
}
