using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class newFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyDays",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudyDays",
                table: "Modules");
        }
    }
}
