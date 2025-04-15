namespace StudentFeeManagement.Model
{
    public class StudentFeeSummary
    {
        public int TotalStudents { get; set; }
        public decimal TotalAdmissionFee { get; set; }
        public decimal TotalAdmissionAmountPaid { get; set; }
        public decimal TotalAdmissionDue { get; set; }

        public decimal TotalFirstTermFee { get; set; }
        public decimal TotalFirstTermAmountPaid { get; set; }
        public decimal TotalFirstTermDue { get; set; }

        public decimal TotalSecondTermFee { get; set; }
        public decimal TotalSecondTermAmountPaid { get; set; }
        public decimal TotalSecondTermDue { get; set; }

        public decimal TotalAnnualFees { get; set; }
        public decimal TotalDues { get; set; }

        public decimal TotalConcession { get; set; }
        public decimal TotalBusFirstTermFee { get; set; }
        public decimal TotalBusFirstTermAmountPaid { get; set; }
        public decimal TotalBusFirstTermDue { get; set; }
        public decimal TotalBusSecondTermFee { get; set; }
        public decimal TotalBusSecondTermAmountPaid { get; set; }
        public decimal TotalBusSecondTermDue { get; set; }
        
    }

}
