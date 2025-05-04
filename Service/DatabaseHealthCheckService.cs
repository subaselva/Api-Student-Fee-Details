using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

namespace StudentFeeManagement.Service
{
    public class DatabaseHealthCheckService
    {
        private readonly AppDbContext _dbContext;
        private readonly RestoreService _restoreService;

        public DatabaseHealthCheckService(AppDbContext dbContext, RestoreService restoreService)
        {
            _dbContext = dbContext;
            _restoreService = restoreService;
        }

        public async Task CheckAndRecoverAsync()
        {
            // Check and recover Students table
            var studentCount = await _dbContext.Students.CountAsync();
            if (studentCount == 0)
            {
                await _restoreService.RestoreStudentsAsync();
                await LogRecoveryActionAsync("Students table");
            }

            // Check and recover StudentFees table
            var studentFeeCount = await _dbContext.StudentFees.CountAsync();
            if (studentFeeCount == 0)
            {
                await _restoreService.RestoreStudentFeesAsync();
                await LogRecoveryActionAsync("StudentFees table");
            }

            // Check and recover DeleteRequests table
            var deleteRequestCount = await _dbContext.DeleteRequests.CountAsync();
            if (deleteRequestCount == 0)
            {
                await _restoreService.RestoreDeleteRequestsAsync();
                await LogRecoveryActionAsync("DeleteRequests table");
            }
        }

        private async Task LogRecoveryActionAsync(string tableName)
        {
            var log = new JobAudit
            {
                JobName = "AutoRecovery",
                Status = "Success",
                ExecutedAt = DateTime.UtcNow,
                Details = $"{tableName} was automatically restored from backup."
            };

            _dbContext.JobAudits.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }


}
