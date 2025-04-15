using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentFeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Studet : Migration
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
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MainstreamDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
