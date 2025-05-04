using Microsoft.AspNetCore.Mvc;
using StudentFeeManagement.Service;

namespace StudentFeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestoreController : ControllerBase
    {
        private readonly RestoreService _restoreService;

        public RestoreController(RestoreService restoreService)
        {
            _restoreService = restoreService;
        }

        [HttpPost("restore-database")]
        public async Task<IActionResult> RestoreDatabase()
        {
            try
            {
                // Trigger all restore methods
                await _restoreService.RestoreStudentsAsync();
                await _restoreService.RestoreStudentFeesAsync();
                await _restoreService.RestoreDeleteRequestsAsync();
                await _restoreService.RestoreAuditLogsAsync();

                return Ok(new { Message = "Database restored successfully." });
            }
            catch (Exception ex)
            {
                // Log error (optional)
                return StatusCode(500, new { Message = "Failed to restore the database.", Error = ex.Message });
            }
        }
    }
}
