namespace StudentFeeManagement.Model
{
    public class AuditLogBackup
    {
        public int Id { get; set; }
        public string ActionType { get; set; }
        public string FullBackupJson { get; set; }
        public DateTime BackupDate { get; set; }
    }

}
