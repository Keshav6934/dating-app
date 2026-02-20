using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyStructureForMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_AppUserId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_AppUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Members");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_Id",
                table: "Members",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_Id",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Members",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_AppUserId",
                table: "Members",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_AppUserId",
                table: "Members",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
