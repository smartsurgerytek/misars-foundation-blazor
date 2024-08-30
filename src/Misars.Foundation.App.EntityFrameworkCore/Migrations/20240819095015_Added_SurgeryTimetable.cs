using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Added_SurgeryTimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSurgeryTimetables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    startdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    enddate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    doctorname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    patientname = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DoctorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PatientId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSurgeryTimetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSurgeryTimetables_AppDoctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "AppDoctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppSurgeryTimetables_AppPatients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AppPatients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSurgeryTimetables_DoctorId",
                table: "AppSurgeryTimetables",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSurgeryTimetables_PatientId",
                table: "AppSurgeryTimetables",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSurgeryTimetables");
        }
    }
}
