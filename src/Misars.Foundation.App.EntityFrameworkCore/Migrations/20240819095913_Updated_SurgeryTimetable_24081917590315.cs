using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SurgeryTimetable_24081917590315 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "patientname",
                table: "AppSurgeryTimetables",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "doctorname",
                table: "AppSurgeryTimetables",
                newName: "AnesthesiaType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notes",
                table: "AppSurgeryTimetables",
                newName: "patientname");

            migrationBuilder.RenameColumn(
                name: "AnesthesiaType",
                table: "AppSurgeryTimetables",
                newName: "doctorname");
        }
    }
}
