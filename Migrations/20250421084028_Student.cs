using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentFeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PermanentEducationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuardianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AadharNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOnAadhar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateMobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherTongue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinorityGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBPLBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAAYBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEWS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCWSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfImpairment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsIndian = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOutOfSchool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDisabilityCertificate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisabilityPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AdmissionNumber = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassRollNo = table.Column<int>(type: "int", nullable: false),
                    MediumOfInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguagesGroupStudied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicGroupStudied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectsGroupStudied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousAcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusPreviousYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Admitted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousAttendance = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacilitiesProvided = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCWSN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSLD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SLDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsASD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsADHD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGifted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInStateCompetition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInNCC_NSS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInUseInternet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Residencetoschool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Educationinfamily = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollments_StudentId",
                table: "StudentEnrollments",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_StudentId",
                table: "StudentProfiles",
                column: "StudentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentEnrollments");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
