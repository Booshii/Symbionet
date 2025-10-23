using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MaiVe.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserIdToSdgTargetsSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b5db594-6508-4cea-adca-1817f6f7f434");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63634ef0-38b8-4cc0-9e82-f0a594b06c60");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "SdgTargets",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f8f80b3-3e65-4705-9128-5d9aaa198afd", null, "Admin", "ADMIN" },
                    { "a17261c5-3b98-4d19-87cd-69b9e0703344", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f8f80b3-3e65-4705-9128-5d9aaa198afd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a17261c5-3b98-4d19-87cd-69b9e0703344");

            migrationBuilder.UpdateData(
                table: "SdgTargets",
                keyColumn: "CreatedByUserId",
                keyValue: null,
                column: "CreatedByUserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "SdgTargets",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b5db594-6508-4cea-adca-1817f6f7f434", null, "Admin", "ADMIN" },
                    { "63634ef0-38b8-4cc0-9e82-f0a594b06c60", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SdgTargets_AspNetUsers_CreatedByUserId",
                table: "SdgTargets",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
