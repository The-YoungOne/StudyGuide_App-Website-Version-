using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class UpdateSemesters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AspNetUsers_UserId",
                table: "Semesters");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Semesters",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Semesters_UserId",
                table: "Semesters",
                newName: "IX_Semesters_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_AspNetUsers_UserName",
                table: "Semesters",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AspNetUsers_UserName",
                table: "Semesters");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Semesters",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Semesters_UserName",
                table: "Semesters",
                newName: "IX_Semesters_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_AspNetUsers_UserId",
                table: "Semesters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
