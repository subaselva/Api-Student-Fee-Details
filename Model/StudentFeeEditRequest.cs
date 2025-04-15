using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentFeeManagement.Model
{
    public class StudentFeeEditRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RegistrationNumber { get; set; }  // Reference to StudentFee

        // Requested changes
        public string? UpdatedStudentName { get; set; }
        public string? UpdatedIsNewStudent { get; set; }
        public string? UpdatedClassSection { get; set; }

        // Admission Fee Changes
        public decimal? UpdatedAdmissionFee { get; set; }
        public decimal? UpdatedAdmissionAmountPaid { get; set; }
        public string? UpdatedAdmissionBillNo { get; set; }
        public DateTime? UpdatedAdmissionDate { get; set; }

        // First Term Fee Changes
        public decimal? UpdatedFirstTermFee { get; set; }
        public decimal? UpdatedFirstTermAmountPaid { get; set; }
        public string? UpdatedFirstTermBillNo { get; set; }
        public DateTime? UpdatedFirstTermDate { get; set; }

        // Second Term Fee Changes
        public decimal? UpdatedSecondTermFee { get; set; }
        public decimal? UpdatedSecondTermAmountPaid { get; set; }
        public string? UpdatedSecondTermBillNo { get; set; }
        public DateTime? UpdatedSecondTermDate { get; set; }

        // Concession & Remarks
        public decimal? UpdatedConcession { get; set; }
        public string? UpdatedRemarks { get; set; }

        // Bus Fees Changes - First Term
        public decimal? UpdatedBusFirstTermFee { get; set; }
        public decimal? UpdatedBusFirstTermAmountPaid { get; set; }

        // Bus Fees Changes - Second Term
        public decimal? UpdatedBusSecondTermFee { get; set; }
        public decimal? UpdatedBusSecondTermAmountPaid { get; set; }

        public string? UpdatedBusPoint { get; set; }

        // Contact Updates
        public string? UpdatedWhatsAppNumber { get; set; }

        // Approval Status
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }

        
    }
}
