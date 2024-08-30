using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Doctor_24082214485073 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notes",
                table: "AppDoctors",
                newName: "Specialty");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AppDoctors",
                newName: "Schedule");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AppDoctors",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorID",
                table: "AppDoctors",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AppDoctors");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "AppDoctors");

            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "AppDoctors",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Schedule",
                table: "AppDoctors",
                newName: "email");
        }
    }
}
