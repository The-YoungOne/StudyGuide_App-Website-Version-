using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class userRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Semesters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Modules",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_UserId",
                table: "Semesters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_UserId",
                table: "Modules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_AspNetUsers_UserId",
                table: "Semesters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AspNetUsers_UserId",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Semesters_UserId",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Modules_UserId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
