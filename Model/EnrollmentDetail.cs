using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentFeeManagement.Model
{
    public class EnrollmentDetail
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; } // Foreign Key

        public int AdmissionNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int ClassRollNo { get; set; }
        public string? MediumOfInstruction { get; set; }

        public string? LanguagesGroupStudied { get; set; }

        public string? AcademicGroupStudied { get; set; }
        public string? SubjectsGroupStudied { get; set; }

        public string? PreviousAcademicYear { get; set; }
        public string? StatusPreviousYear { get; set; }

        public string? Admitted { get; set; }
        public string? PreviousClass { get; set; }

        public string? Marks { get; set; }
        public string? PreviousAttendance { get; set; }
        
    }
}
