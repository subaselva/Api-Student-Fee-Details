namespace StudentFeeManagement.Model
{
    public class DeleteRequest
    {
        public int Id { get; set; }
        public int StudentFeeId { get; set; }
        public string StudentName { get; set; }
        public DateTime RequestedAt { get; set; }
        public string RequestedBy { get; set; } // Optional: track who requested it
        public bool Approved { get; set; } = false;
    }

}
