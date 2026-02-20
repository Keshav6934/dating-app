using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema_19_02_2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "d9da4adc-3aa9-48b7-8377-aaf14c0ca957");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "member-id",
                column: "ConcurrencyStamp",
                value: "42bfa5b2-670d-40ad-8e0c-8f546340cb6f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator-id",
                column: "ConcurrencyStamp",
                value: "660e003c-327b-4d10-9c51-e4aee6e0ba64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "17fbc24d-8bbd-4e9c-9018-51612be26c72");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "member-id",
                column: "ConcurrencyStamp",
                value: "b16f10bf-5c31-4883-8abe-81a513e95811");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator-id",
                column: "ConcurrencyStamp",
                value: "99ba2db2-5e7b-4a91-a527-bcebfc8507db");
        }
    }
}
