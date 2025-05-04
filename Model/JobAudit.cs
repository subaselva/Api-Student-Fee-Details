namespace StudentFeeManagement.Model
{
    public class JobAudit
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string Status { get; set; } // Success / Failed
        public DateTime ExecutedAt { get; set; }
        public string? Details { get; set; }
    }
}
