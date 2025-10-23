using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaiVe.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SdgTargetSdgTypes_SdgTypes_SdgTypeId",
                table: "SdgTargetSdgTypes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "700eab9f-7fb4-48e4-ab9b-4e316827a0b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2449f6e-52ea-460f-ba80-c5a0725198c5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "459a0d16-fa74-4de4-b723-8302fc5d5a5e", null, "User", "USER" },
                    { "84758956-3dcd-48b5-9ec7-8f29b52857cc", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SdgTargetSdgTypes_SdgTypes_SdgTypeId",
                table: "SdgTargetSdgTypes",
                column: "SdgTypeId",
                principalTable: "SdgTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SdgTargetSdgTypes_SdgTypes_SdgTypeId",
                table: "SdgTargetSdgTypes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "459a0d16-fa74-4de4-b723-8302fc5d5a5e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84758956-3dcd-48b5-9ec7-8f29b52857cc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "700eab9f-7fb4-48e4-ab9b-4e316827a0b3", null, "Admin", "ADMIN" },
                    { "f2449f6e-52ea-460f-ba80-c5a0725198c5", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SdgTargetSdgTypes_SdgTypes_SdgTypeId",
                table: "SdgTargetSdgTypes",
                column: "SdgTypeId",
                principalTable: "SdgTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
