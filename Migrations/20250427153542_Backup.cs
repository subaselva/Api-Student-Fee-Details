using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentFeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Backup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogBackups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullBackupJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogBackups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeleteRequestBackups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullBackupJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteRequestBackups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentBackups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullBackupJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBackups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentFeeBackups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullBackupJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeBackups", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogBackups");

            migrationBuilder.DropTable(
                name: "DeleteRequestBackups");

            migrationBuilder.DropTable(
                name: "StudentBackups");

            migrationBuilder.DropTable(
                name: "StudentFeeBackups");
        }
    }
}
