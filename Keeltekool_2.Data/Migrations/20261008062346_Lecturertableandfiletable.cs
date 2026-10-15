using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keeltekool_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Lecturertableandfiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageCourses",
                table: "LanguageCourses");

            migrationBuilder.RenameTable(
                name: "LanguageCourses",
                newName: "LanguageCourse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageCourse",
                table: "LanguageCourse",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageCourse",
                table: "LanguageCourse");

            migrationBuilder.RenameTable(
                name: "LanguageCourse",
                newName: "LanguageCourses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageCourses",
                table: "LanguageCourses",
                column: "Id");
        }
    }
}
