using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StudentFeeManagement.Service
{
    public class RestoreService
    {
        private readonly AppDbContext _dbContext;
        private readonly string _backupFolderPath = @"C:\Backup"; // Set your local backup folder path

        public RestoreService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            // Ensure backup folder and subfolders exist
            EnsureBackupDirectoryExists();
        }

        private void EnsureBackupDirectoryExists()
        {
            var studentDirectory = Path.Combine(_backupFolderPath, "Students");
            var feeDirectory = Path.Combine(_backupFolderPath, "StudentFees");
            var requestDirectory = Path.Combine(_backupFolderPath, "DeleteRequests");
            var logDirectory = Path.Combine(_backupFolderPath, "AuditLogs");

            // Ensure directories exist, create if necessary
            Directory.CreateDirectory(studentDirectory);
            Directory.CreateDirectory(feeDirectory);
            Directory.CreateDirectory(requestDirectory);
            Directory.CreateDirectory(logDirectory);
        }

        public async Task RestoreAllAsync()
        {
            await RestoreStudentsAsync();
            await RestoreStudentFeesAsync();
            await RestoreDeleteRequestsAsync();
            await RestoreAuditLogsAsync();
        }

        public async Task RestoreStudentsAsync()
        {
            var studentFiles = Directory.GetFiles(Path.Combine(_backupFolderPath, "Students"), "*.json");

            foreach (var filePath in studentFiles)
            {
                var fileName = Path.GetFileName(filePath);
                var rollNumber = fileName.Split('_')[1]; // Assuming the file name format is `Student_<RollNumber>_<Timestamp>.json`

                var json = await File.ReadAllTextAsync(filePath);
                var student = JsonConvert.DeserializeObject<Student>(json);

                if (student != null)
                {
                    student.Id = 0; // Reset identity
                    if (student.Enrollment != null) student.Enrollment.Id = 0;
                    if (student.Profile != null) student.Profile.Id = 0;

                    var existingStudent = await _dbContext.Students
                        .Include(s => s.Enrollment)
                        .Include(s => s.Profile)
                        .FirstOrDefaultAsync(s => s.RollNumber == rollNumber);

                    if (existingStudent == null)
                    {
                        _dbContext.Students.Add(student);
                    }
                    else
                    {
                        _dbContext.Entry(existingStudent).CurrentValues.SetValues(student);

                        if (student.Enrollment != null)
                        {
                            _dbContext.Entry(existingStudent.Enrollment).CurrentValues.SetValues(student.Enrollment);
                        }

                        if (student.Profile != null)
                        {
                            _dbContext.Entry(existingStudent.Profile).CurrentValues.SetValues(student.Profile);
                        }
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RestoreStudentFeesAsync()
        {
            var feeFiles = Directory.GetFiles(Path.Combine(_backupFolderPath, "StudentFees"), "*.json");

            foreach (var filePath in feeFiles)
            {
                var json = await File.ReadAllTextAsync(filePath);
                var fee = JsonConvert.DeserializeObject<StudentFee>(json);

                if (fee != null)
                {
                    fee.RegistrationNumber = 0;

                    var existingFee = await _dbContext.StudentFees
                        .FirstOrDefaultAsync(x => x.RegistrationNumber == fee.RegistrationNumber);

                    if (existingFee == null)
                    {
                        _dbContext.StudentFees.Add(fee);
                    }
                    else
                    {
                        _dbContext.Entry(existingFee).CurrentValues.SetValues(fee);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RestoreDeleteRequestsAsync()
        {
            var requestFiles = Directory.GetFiles(Path.Combine(_backupFolderPath, "DeleteRequests"), "*.json");

            foreach (var filePath in requestFiles)
            {
                var json = await File.ReadAllTextAsync(filePath);
                var request = JsonConvert.DeserializeObject<DeleteRequest>(json);

                if (request != null)
                {
                    var existingRequest = await _dbContext.DeleteRequests
                        .FirstOrDefaultAsync(x => x.Id == request.Id);

                    if (existingRequest == null)
                    {
                        _dbContext.DeleteRequests.Add(request);
                    }
                    else
                    {
                        _dbContext.Entry(existingRequest).CurrentValues.SetValues(request);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RestoreAuditLogsAsync()
        {
            var logFiles = Directory.GetFiles(Path.Combine(_backupFolderPath, "AuditLogs"), "*.json");

            foreach (var filePath in logFiles)
            {
                var json = await File.ReadAllTextAsync(filePath);
                var log = JsonConvert.DeserializeObject<AuditLog>(json);

                if (log != null)
                {
                    var existingLog = await _dbContext.AuditLogs
                        .FirstOrDefaultAsync(x => x.Id == log.Id);

                    if (existingLog == null)
                    {
                        _dbContext.AuditLogs.Add(log);
                    }
                    else
                    {
                        _dbContext.Entry(existingLog).CurrentValues.SetValues(log);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
