using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Misars.Foundation.App.Migrations
{
    /// <inheritdoc />
    public partial class Added_FileManagement_Module_51791 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FmDirectoryDescriptors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FmDirectoryDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FmDirectoryDescriptors_FmDirectoryDescriptors_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FmDirectoryDescriptors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FmFileDescriptors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DirectoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Size = table.Column<int>(type: "INTEGER", maxLength: 2147483647, nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FmFileDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FmFileDescriptors_FmDirectoryDescriptors_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "FmDirectoryDescriptors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FmDirectoryDescriptors_ParentId",
                table: "FmDirectoryDescriptors",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FmDirectoryDescriptors_TenantId_ParentId_Name",
                table: "FmDirectoryDescriptors",
                columns: new[] { "TenantId", "ParentId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FmFileDescriptors_DirectoryId",
                table: "FmFileDescriptors",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FmFileDescriptors_TenantId_DirectoryId_Name",
                table: "FmFileDescriptors",
                columns: new[] { "TenantId", "DirectoryId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FmFileDescriptors");

            migrationBuilder.DropTable(
                name: "FmDirectoryDescriptors");
        }
    }
}
