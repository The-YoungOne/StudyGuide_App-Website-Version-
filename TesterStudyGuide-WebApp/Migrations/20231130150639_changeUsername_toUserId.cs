using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class changeUsername_toUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AspNetUsers_UserName",
                table: "Semesters");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Semesters",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Semesters_UserName",
                table: "Semesters",
                newName: "IX_Semesters_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_AspNetUsers_Id",
                table: "Semesters",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AspNetUsers_Id",
                table: "Semesters");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Semesters",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Semesters_Id",
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
    }
}
