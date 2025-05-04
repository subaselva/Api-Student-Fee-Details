using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StudentFeeManagement.Service
{
    public class DataBackupService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _backupDirectory = @"C:\Backup"; // Local backup directory

        public DataBackupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BackupAllAsync()
        {
            await BackupStudentsAsync();
            await BackupStudentFeesAsync();
            await BackupDeleteRequestsAsync();
            await BackupAuditLogsAsync();
        }

        private async Task BackupStudentsAsync()
        {
            var students = await _dbContext.Students
                .Include(s => s.Enrollment)
                .Include(s => s.Profile)
                .ToListAsync();

            // Create the backup directory if it doesn't exist
            Directory.CreateDirectory(_backupDirectory);

            foreach (var student in students)
            {
                var jsonData = JsonConvert.SerializeObject(student, Formatting.Indented);
                string fileName = Path.Combine(_backupDirectory, $"Student_{student.RollNumber}_{DateTime.UtcNow:yyyyMMddHHmmss}.json");

                // Write the data to a local file
                await File.WriteAllTextAsync(fileName, jsonData);

                // Log the backup action
                await LogJobAsync("BackupStudents", "Success", $"Backed up {students.Count} students to local file storage");
            }
        }

        private async Task BackupStudentFeesAsync()
        {
            var fees = await _dbContext.StudentFees.ToListAsync();

            // Create the backup directory if it doesn't exist
            Directory.CreateDirectory(_backupDirectory);

            foreach (var fee in fees)
            {
                var jsonData = JsonConvert.SerializeObject(fee, Formatting.Indented);
                string fileName = Path.Combine(_backupDirectory, $"StudentFee_{fee.StudentName}_{DateTime.UtcNow:yyyyMMddHHmmss}.json");

                // Write the data to a local file
                await File.WriteAllTextAsync(fileName, jsonData);
            }

            // Log the backup action
            await LogJobAsync("BackupStudentFees", "Success", $"Backed up {fees.Count} student fees to local file storage");
        }

        private async Task BackupDeleteRequestsAsync()
        {
            var requests = await _dbContext.DeleteRequests.ToListAsync();

            // Create the backup directory if it doesn't exist
            Directory.CreateDirectory(_backupDirectory);

            foreach (var request in requests)
            {
                var jsonData = JsonConvert.SerializeObject(request, Formatting.Indented);
                string fileName = Path.Combine(_backupDirectory, $"DeleteRequest_{request.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.json");

                // Write the data to a local file
                await File.WriteAllTextAsync(fileName, jsonData);
            }

            // Log the backup action
            await LogJobAsync("BackupDeleteRequests", "Success", $"Backed up {requests.Count} delete requests to local file storage");
        }

        private async Task BackupAuditLogsAsync()
        {
            var logs = await _dbContext.AuditLogs.ToListAsync();

            // Create the backup directory if it doesn't exist
            Directory.CreateDirectory(_backupDirectory);

            foreach (var log in logs)
            {
                var jsonData = JsonConvert.SerializeObject(log, Formatting.Indented);
                string fileName = Path.Combine(_backupDirectory, $"AuditLog_{log.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.json");

                // Write the data to a local file
                await File.WriteAllTextAsync(fileName, jsonData);
            }

            // Log the backup action
            await LogJobAsync("BackupAuditLogs", "Success", $"Backed up {logs.Count} audit logs to local file storage");
        }

        private async Task LogJobAsync(string jobName, string status, string? details = null)
        {
            var log = new JobAudit
            {
                JobName = jobName,
                Status = status,
                ExecutedAt = DateTime.UtcNow,
                Details = details
            };

            _dbContext.JobAudits.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
