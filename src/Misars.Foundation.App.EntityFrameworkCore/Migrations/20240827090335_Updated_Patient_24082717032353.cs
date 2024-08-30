using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Patient_24082717032353 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "ContactInformation",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "CurrentMedications",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "EmergencyConstact",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "InsuranceInformation",
                table: "AppPatients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInformation",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentMedications",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyConstact",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceInformation",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }
    }
}
