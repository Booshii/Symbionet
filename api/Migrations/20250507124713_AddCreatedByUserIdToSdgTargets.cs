using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaiVe.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserIdToSdgTargets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5325d50a-8a61-43a2-a31a-e7ad1b65c03e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8f8fa2d-4f61-432b-b416-941e40542942");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SdgTargets",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b5db594-6508-4cea-adca-1817f6f7f434", null, "Admin", "ADMIN" },
                    { "63634ef0-38b8-4cc0-9e82-f0a594b06c60", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SdgTargets_CreatedByUserId",
                table: "SdgTargets",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets");

            migrationBuilder.DropIndex(
                name: "IX_SdgTargets_CreatedByUserId",
                table: "SdgTargets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b5db594-6508-4cea-adca-1817f6f7f434");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63634ef0-38b8-4cc0-9e82-f0a594b06c60");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SdgTargets");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5325d50a-8a61-43a2-a31a-e7ad1b65c03e", null, "Admin", "ADMIN" },
                    { "f8f8fa2d-4f61-432b-b416-941e40542942", null, "User", "USER" }
                });
        }
    }
}
