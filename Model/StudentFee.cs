using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentFeeManagement.Model
{
    public class StudentFee
    {
        [Key]
        public int RegistrationNumber { get; set; }  // Unique Student ID

        [Required]
        public string StudentName { get; set; } = string.Empty;

        public string? IsNewStudent { get; set; }  // New/Old Status

        public string? ClassSection { get; set; }  // a, b, c, etc.

        // Admission Fee Details
        public decimal? AdmissionFee { get; set; }
        public decimal? AdmissionAmountPaid { get; set; }

        [NotMapped]
        public decimal? AdmissionDue => (AdmissionFee ?? 0) - (AdmissionAmountPaid ?? 0);

        public string? AdmissionBillNo { get; set; }
        public DateTime? AdmissionDate { get; set; }

        // First Term Fee
        public decimal? FirstTermFee { get; set; }
        public decimal? FirstTermAmountPaid { get; set; }

        [NotMapped]
        public decimal? FirstTermDue => (FirstTermFee ?? 0) - (FirstTermAmountPaid ?? 0);

        public string? FirstTermBillNo { get; set; }
        public DateTime? FirstTermDate { get; set; }

        // Second Term Fee
        public decimal? SecondTermFee { get; set; }
        public decimal? SecondTermAmountPaid { get; set; }

        [NotMapped]
        public decimal? SecondTermDue => (SecondTermFee ?? 0) - (SecondTermAmountPaid ?? 0);

        public string? SecondTermBillNo { get; set; }
        public DateTime? SecondTermDate { get; set; }

        // Annual & Total Dues Calculation
        [NotMapped]
        public decimal? AnnualFees => (AdmissionFee ?? 0) + (FirstTermFee ?? 0) + (SecondTermFee ?? 0);

        [NotMapped]
        public decimal? Dues => (AdmissionDue ?? 0) + (FirstTermDue ?? 0) + (SecondTermDue ?? 0);

        // Concession & Remarks
        public decimal? Concession { get; set; }
        public string? Remarks { get; set; }

        // Bus Fees - First Term
        public decimal? BusFirstTermFee { get; set; }
        public decimal? BusFirstTermAmountPaid { get; set; }

        [NotMapped]
        public decimal? BusFirstTermDue => (BusFirstTermFee ?? 0) - (BusFirstTermAmountPaid ?? 0);

        // Bus Fees - Second Term
        public decimal? BusSecondTermFee { get; set; }
        public decimal? BusSecondTermAmountPaid { get; set; }

        [NotMapped]
        public decimal? BusSecondTermDue => (BusSecondTermFee ?? 0) - (BusSecondTermAmountPaid ?? 0);

        // Additional Details
        public string? BusPoint { get; set; }

        [Required]
        public string WhatsAppNumber { get; set; } 
       
    }
}
