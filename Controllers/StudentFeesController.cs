using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Channels;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class StudentFeesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuditService _auditService;
   // private readonly IHubContext<NotificationHub> _hubContext;

    public StudentFeesController(AppDbContext context, AuditService auditService)
    {
        _context = context;
        _auditService = auditService;
       // _hubContext = hubContext;
    }
    [HttpGet("GetFeesByDate")]
    public async Task<IActionResult> GetFeesByDate([FromQuery] DateTime selectedDate)
    {
        var fees = await _context.StudentFees
            .Where(f => EF.Functions.DateDiffDay(f.AdmissionDate, selectedDate) == 0 ||
                        EF.Functions.DateDiffDay(f.FirstTermDate, selectedDate) == 0 ||
                        EF.Functions.DateDiffDay(f.SecondTermDate, selectedDate) == 0)
            .Select(f => new
            {
                f.RegistrationNumber,
                f.StudentName,
                AdmissionAmountPaid = EF.Functions.DateDiffDay(f.AdmissionDate, selectedDate) == 0 ? f.AdmissionAmountPaid : null,
                AdmissionFee = EF.Functions.DateDiffDay(f.AdmissionDate, selectedDate) == 0 ? f.AdmissionFee : null,
                FirstTermAmountPaid = EF.Functions.DateDiffDay(f.FirstTermDate, selectedDate) == 0 ? f.FirstTermAmountPaid : null,
                FirstTermFee = EF.Functions.DateDiffDay(f.FirstTermDate, selectedDate) == 0 ? f.FirstTermFee : null,
                SecondTermAmountPaid = EF.Functions.DateDiffDay(f.SecondTermDate, selectedDate) == 0 ? f.SecondTermAmountPaid : null,
                SecondTermFee = EF.Functions.DateDiffDay(f.SecondTermDate, selectedDate) == 0 ? f.SecondTermFee : null,
            })
            .ToListAsync();

        if (!fees.Any())
            return NotFound("No fees found for the selected date.");

        return Ok(fees);
    }



    // GET: api/StudentFees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentFee>>> GetStudentFees()
    {
        return await _context.StudentFees.ToListAsync();
    }

    // GET: api/StudentFees/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentFee>> GetStudentFee(int id)
    {
        var studentFee = await _context.StudentFees.FindAsync(id);
        if (studentFee == null)
        {
            return NotFound();
        }
        return studentFee;
    }
    [HttpGet("pending-edits")]
    public async Task<IActionResult> GetPendingEditRequests()
    {
        // Fetch pending requests from the database that have some updated fields
        var requests = await _context.StudentFeeEditRequests
                                      .Where(r => r.Status == "Pending" &&
                                                  (r.UpdatedStudentName != null || r.UpdatedIsNewStudent != null ||
                                                   r.UpdatedClassSection != null || r.UpdatedAdmissionFee != null ||
                                                   r.UpdatedAdmissionAmountPaid != null || r.UpdatedAdmissionBillNo != null ||
                                                   r.UpdatedAdmissionDate != null || r.UpdatedFirstTermFee != null ||
                                                   r.UpdatedFirstTermAmountPaid != null || r.UpdatedFirstTermBillNo != null ||
                                                   r.UpdatedFirstTermDate != null || r.UpdatedSecondTermFee != null ||
                                                   r.UpdatedSecondTermAmountPaid != null || r.UpdatedSecondTermBillNo != null ||
                                                   r.UpdatedSecondTermDate != null || r.UpdatedConcession != null ||
                                                   r.UpdatedRemarks != null || r.UpdatedBusFirstTermFee != null ||
                                                   r.UpdatedBusFirstTermAmountPaid != null || r.UpdatedBusSecondTermFee != null ||
                                                   r.UpdatedBusSecondTermAmountPaid != null || r.UpdatedBusPoint != null ||
                                                   r.UpdatedWhatsAppNumber != null)) // Check for any updates
                                      .ToListAsync();

        // Iterate through the requests and process them if they have updates
        foreach (var request in requests)
        {
            // Check if the request has already been created by looking for duplicate registrationNumber and status
            var existingRequest = await _context.StudentFeeEditRequests
                                                 .Where(r => r.RegistrationNumber == request.RegistrationNumber
                                                             && r.Status == "Pending"
                                                             && r.Id != request.Id) // Ensure it's not the same request
                                                 .FirstOrDefaultAsync();

            if (existingRequest == null)
            {
                // Process or update the request as needed
                // For example, you can modify any of the fields here based on business logic or simply leave it for review

                // Send notification to CEO about pending requests
                var message = "You have new pending fee edit requests.";
               // await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

                // Save any other changes or processed requests
                await _context.SaveChangesAsync();
            }
        }

        return Ok(requests);
    }



   
    // POST: api/StudentFees
    [HttpPost]
    public async Task<IActionResult> CreateStudentFee([FromBody] StudentFee studentFee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.StudentFees.Add(studentFee);
        await _context.SaveChangesAsync();

        // ✅ Log the created student fee details
        await _auditService.LogAction(
            AuditAction.Created,  // Enum for "Created" action
            "StudentFees",
            studentFee.RegistrationNumber.ToString(),
            newStudentFee: studentFee // Pass newStudentFee to store details in the audit log
        );

        return Ok(studentFee);
    }

    [HttpPut("edit-request")]
    public async Task<IActionResult> SubmitEditRequest([FromBody] StudentFeeEditRequest request)
    {
        var existingFee = await _context.StudentFees.FindAsync(request.RegistrationNumber);
        if (existingFee == null)
            return NotFound("Student fee record not found.");

        var oldRequest = await _context.StudentFeeEditRequests
            .FirstOrDefaultAsync(r => r.RegistrationNumber == request.RegistrationNumber);

        var changes = new Dictionary<string, object>();

        if (oldRequest != null)
        {
            var properties = typeof(StudentFeeEditRequest).GetProperties();
            foreach (var prop in properties)
            {
                var oldValue = prop.GetValue(oldRequest);
                var newValue = prop.GetValue(request);

                if (newValue != null && !object.Equals(oldValue, newValue)) // ✅ Detect changes
                {
                    changes[prop.Name] = newValue;
                }
            }

            _context.StudentFeeEditRequests.Remove(oldRequest);
        }
        else
        {
            // ✅ If it's the first edit request, treat all fields as changes
            var properties = typeof(StudentFeeEditRequest).GetProperties();
            foreach (var prop in properties)
            {
                var newValue = prop.GetValue(request);
                if (newValue != null)
                {
                    changes[prop.Name] = newValue;
                }
            }
        }

        request.Status = "Pending";
        _context.StudentFeeEditRequests.Add(request);
        await _context.SaveChangesAsync();

        // ✅ Log only when there is at least one change
  //      if (changes.Count > 0)
   //     {
            changes["Id"] = request.Id; // Include ID in audit log

            await _auditService.LogAction(
                AuditAction.Updated,
                "StudentFeeEditRequest",
                request.RegistrationNumber.ToString(),
                changes: changes
            );
  //      }
  //      else
 //       {
   //         return BadRequest("No changes detected.");
   //     }

        return Ok("Edit request submitted successfully.");
    }




    [HttpPost("approve-edit/{id}")]
    public async Task<IActionResult> ApproveEditRequest(int id)
    {
        var request = await _context.StudentFeeEditRequests.FindAsync(id);
        if (request == null || request.Status != "Pending")
        {
            return NotFound("Edit request not found or already processed.");
        }

        var studentFee = await _context.StudentFees.FindAsync(request.RegistrationNumber);
        if (studentFee == null)
        {
            return NotFound("Student fee record not found.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var approvedFields = new Dictionary<string, object>();

            // Apply changes only to modified fields
            foreach (var property in typeof(StudentFee).GetProperties())
            {
                var updatedValue = typeof(StudentFeeEditRequest).GetProperty("Updated" + property.Name)?.GetValue(request);
                var originalValue = property.GetValue(studentFee);

                if (updatedValue != null && !Equals(originalValue, updatedValue)) // ✅ Log only changed fields
                {
                    approvedFields[property.Name] = new { OldValue = originalValue, NewValue = updatedValue };
                    property.SetValue(studentFee, updatedValue);
                }
            }

            _context.StudentFees.Update(studentFee);

            // ✅ Mark request as approved
            request.Status = "Approved";
            request.ApprovedDate = DateTime.UtcNow;
            request.ApprovedBy = "CEO"; // Replace with actual user identity

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // ✅ Log only the approved changes
            if (approvedFields.Count > 0) // Only log if there are actual changes
            {
                await _auditService.LogAction(
                    AuditAction.Approved,
                    "StudentFeeEditRequest",
                    request.RegistrationNumber.ToString(),
                    changes: approvedFields
                );
            }

            // ✅ Return only the updated fields + Student ID
            return Ok(new
            {
                Message = "Edit request approved and updated in the database.",
                StudentID = request.Id,  // Always include Registration Number
                ApprovedFields = approvedFields          // Only approved fields with old & new values
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error processing request: {ex.Message}");
        }
    }



    [HttpPost("reject-edit/{id}")]
    public async Task<IActionResult> RejectEditRequest(int id)
    {
        var request = await _context.StudentFeeEditRequests.FindAsync(id);
        if (request == null || request.Status != "Pending")
            return NotFound("Edit request not found or already processed.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // ✅ Track only modified and rejected fields with both old and new values
            var rejectedFields = new Dictionary<string, object>();

            foreach (var prop in typeof(StudentFeeEditRequest).GetProperties())
            {
                if (prop.Name.StartsWith("Updated"))
                {
                    var updatedValue = prop.GetValue(request);
                    if (updatedValue != null) // ✅ Check if field was actually edited
                    {
                        string originalFieldName = prop.Name.Replace("Updated", "");

                        // ✅ Fetch the original value from StudentFee
                        var studentFee = await _context.StudentFees.FindAsync(request.RegistrationNumber);
                        if (studentFee != null)
                        {
                            var originalValue = typeof(StudentFee).GetProperty(originalFieldName)?.GetValue(studentFee);

                            // ✅ Only log fields where the edited value is different from the original
                            if (!Equals(updatedValue, originalValue))
                            {
                                rejectedFields[originalFieldName] = new
                                {
                                    FieldId = request.Id,
                                    FieldName = originalFieldName,
                                    OriginalValue = originalValue,
                                    RejectedValue = updatedValue
                                };
                            }
                        }
                    }
                }
            }

            // ✅ Mark request as rejected
            request.Status = "Rejected";
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // ✅ Log rejection only if there are rejected fields
            if (rejectedFields.Any())
            {
                await _auditService.LogAction(
                    AuditAction.Rejected,
                    "StudentFeeEditRequest",
                    request.RegistrationNumber.ToString(),
                    changes: rejectedFields // Only log the rejected fields with their old and new values
                );
            }

            return Ok(new { Message = "Edit request rejected successfully.", RejectedFields = rejectedFields });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error processing rejection: {ex.Message}");
        }
    }


    // PUT: api/StudentFees/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudentFee(int id, StudentFee studentFee)
    {
        if (id != studentFee.RegistrationNumber)
        {
            return BadRequest();
        }

        var existingFee = await _context.StudentFees.AsNoTracking().FirstOrDefaultAsync(f => f.RegistrationNumber == id);
        if (existingFee == null)
        {
            return NotFound();
        }

        _context.Entry(studentFee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            await _auditService.LogAction(
                AuditAction.Updated,  // ✅ Corrected: Use enum instead of string
                "StudentFees",
                studentFee.RegistrationNumber.ToString(),
                studentFee,
                existingFee
            );
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.StudentFees.Any(e => e.RegistrationNumber == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/StudentFees/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentFee(int id)
    {
        var studentFee = await _context.StudentFees.FindAsync(id);
        if (studentFee == null)
        {
            return NotFound();
        }

        _context.StudentFees.Remove(studentFee);
        await _context.SaveChangesAsync();

        await _auditService.LogAction(
            AuditAction.Deleted,  // ✅ Corrected: Use enum instead of string
            "StudentFees",
            studentFee.RegistrationNumber.ToString(),
            studentFee
        );

        return NoContent();
    }

    // ✅ NEW: Get Summary of Student Fees
    [HttpGet("Summary")]
    public async Task<IActionResult> GetStudentFeeSummary()
    {
        var summary = await _context.GetStudentFeeSummaryAsync();
        return Ok(summary);
    }
    [HttpGet("debug-user")]
    public IActionResult DebugUserClaims()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(claims);
    }

}
