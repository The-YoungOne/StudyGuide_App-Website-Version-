using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class updateRecordsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Records",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Records_Id",
                table: "Records",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_AspNetUsers_Id",
                table: "Records",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_AspNetUsers_Id",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_Id",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Records");
        }
    }
}
