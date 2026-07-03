using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddClassToTeacherSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "TeacherSubjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_ClassId",
                table: "TeacherSubjects",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Classes_ClassId",
                table: "TeacherSubjects",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Classes_ClassId",
                table: "TeacherSubjects");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjects_ClassId",
                table: "TeacherSubjects");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "TeacherSubjects");
        }
    }
}
