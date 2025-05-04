using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentFeeManagement.Model
{
    public class StudentProfile
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; } // Foreign Key

        public string StudentName { get; set; }
        public string? FacilitiesProvided { get; set; }
        public string? IsCWSN { get; set; }
        public string? IsSLD { get; set; }
        public string? SLDType { get; set; }
        public string? IsASD { get; set; }
        public string? IsADHD { get; set; }
        public string? IsGifted { get; set; }
        public string? IsInStateCompetition { get; set; }
        public string? IsInNCC_NSS { get; set; }
        public string? IsInUseInternet { get; set; }
        public string? Height { get; set; }

        public string? Weight { get; set; }
        public string? Residencetoschool { get; set; }
        public string? Educationinfamily { get; set; }
    }
}
