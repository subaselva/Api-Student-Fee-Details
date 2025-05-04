using Microsoft.AspNetCore.Mvc;
using StudentFeeManagement.Service;

namespace StudentFeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase
    {
        private readonly DataBackupService _backupService;

        public BackupController(DataBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunBackup()
        {
            await _backupService.BackupAllAsync();
            return Ok("Backup completed successfully.");
        }
    }
}
