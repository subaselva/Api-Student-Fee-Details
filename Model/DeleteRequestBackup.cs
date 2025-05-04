namespace StudentFeeManagement.Model
{
    public class DeleteRequestBackup
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public string FullBackupJson { get; set; }
        public DateTime BackupDate { get; set; }
    }

}
