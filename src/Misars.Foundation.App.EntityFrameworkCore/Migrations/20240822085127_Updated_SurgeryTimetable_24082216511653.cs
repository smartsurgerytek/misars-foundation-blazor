using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SurgeryTimetable_24082216511653 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingrRoomNumber",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurgicalMethod",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ward",
                table: "AppSurgeryTimetables",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "AppSurgeryTimetables");

            migrationBuilder.DropColumn(
                name: "OperatingrRoomNumber",
                table: "AppSurgeryTimetables");

            migrationBuilder.DropColumn(
                name: "SurgicalMethod",
                table: "AppSurgeryTimetables");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "AppSurgeryTimetables");

            migrationBuilder.DropColumn(
                name: "ward",
                table: "AppSurgeryTimetables");
        }
    }
}
