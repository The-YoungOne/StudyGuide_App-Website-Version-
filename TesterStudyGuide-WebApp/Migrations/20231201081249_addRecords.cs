using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesterStudyGuide_WebApp.Migrations
{
    public partial class addRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "totalStudyHours",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    calendar_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hoursStudied = table.Column<double>(type: "float", nullable: false),
                    code = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.calendar_id);
                    table.ForeignKey(
                        name: "FK_Records_Modules_code",
                        column: x => x.code,
                        principalTable: "Modules",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_code",
                table: "Records",
                column: "code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropColumn(
                name: "totalStudyHours",
                table: "Modules");
        }
    }
}
