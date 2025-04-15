using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;


[Route("api/[controller]")]
[ApiController]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetAuditLogs")]
    public async Task<IActionResult> GetAuditLogs()
    {
        var logs = await _context.AuditLogs.OrderByDescending(a => a.Timestamp).ToListAsync();
        return Ok(logs);
    }
}
