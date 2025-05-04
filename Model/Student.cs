using System.ComponentModel.DataAnnotations;

namespace StudentFeeManagement.Model
{

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RollNumber { get; set; } // Global Unique Identifier for Student

        [Required]
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string PermanentEducationNumber { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string GuardianName { get; set; }
        public string AadharNumber { get; set; }
        public string NameOnAadhar { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string AlternateMobileNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }


        // Newly Added Fields
        public string MotherTongue { get; set; }
        public string SocialCategory { get; set; }
        public string MinorityGroup { get; set; }
        public string IsBPLBeneficiary { get; set; }
        public string IsAAYBeneficiary { get; set; }
        public string IsEWS { get; set; }
        public string IsCWSN { get; set; }
        public string TypeOfImpairment { get; set; }
        public string IsIndian { get; set; }
        public string IsOutOfSchool { get; set; }

        public string HasDisabilityCertificate { get; set; }
        public Decimal DisabilityPercentage { get; set; }
        public string BloodGroup { get; set; }

        // Navigation properties
    public EnrollmentDetail Enrollment { get; set; }
    public StudentProfile Profile { get; set; }
    }


}
