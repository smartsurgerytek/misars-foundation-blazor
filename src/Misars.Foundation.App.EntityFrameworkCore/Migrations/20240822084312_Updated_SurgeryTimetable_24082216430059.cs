using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SurgeryTimetable_24082216430059 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notes",
                table: "AppSurgeryTimetables",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "AnesthesiaType",
                table: "AppSurgeryTimetables",
                newName: "Diagnosis");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AppSurgeryTimetables");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "AppSurgeryTimetables",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "AppSurgeryTimetables",
                newName: "AnesthesiaType");
        }
    }
}
