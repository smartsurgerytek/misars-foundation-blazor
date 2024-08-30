using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Patient_24082215022438 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "AppPatients");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "AppPatients",
                newName: "PatientID");

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
                name: "DateofBirth",
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
                name: "Gender",
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

            migrationBuilder.AddColumn<string>(
                name: "MedicalHistory",
                table: "AppPatients",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "DateofBirth",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "EmergencyConstact",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "InsuranceInformation",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "MedicalHistory",
                table: "AppPatients");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "AppPatients",
                newName: "phone");

            migrationBuilder.AddColumn<DateTime>(
                name: "birthday",
                table: "AppPatients",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
