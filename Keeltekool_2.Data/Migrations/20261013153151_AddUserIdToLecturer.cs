using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keeltekool_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LecturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.ImageID);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Lecturers");

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
    }
}
