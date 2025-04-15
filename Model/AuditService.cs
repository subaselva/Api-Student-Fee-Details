using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

public class AuditService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Logs an audit entry for Create, Update, or Delete actions.
    /// </summary>
    public async Task LogAction(AuditAction action, string entity, string entityId, StudentFee? newStudentFee, StudentFee? oldStudentFee = null)
    {
        if (entity == "StudentFeeEditRequest") return; //  Ignore edit requests

        var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";

        Dictionary<string, object> changes = action == AuditAction.Deleted
            ? new Dictionary<string, object> { { "Deleted Entity", oldStudentFee ?? newStudentFee! } }
            : GetModifiedFields(oldStudentFee, newStudentFee!);

        if (changes.Count == 0 && action != AuditAction.Created) return;

        var auditLog = new AuditLog
        {
            UserEmail = userEmail,
            Action = action.ToString(),
            Entity = entity,
            Timestamp = DateTime.UtcNow,
            Changes = newStudentFee != null
            ? JsonSerializer.Serialize(newStudentFee) // ✅ Store full details of created entity
            : oldStudentFee != null
                ? JsonSerializer.Serialize(oldStudentFee)
                : null
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
    public async Task LogAction(AuditAction action, string entity, string entityId,
    StudentFeeEditRequest? newRequest = null,
    StudentFeeEditRequest? oldRequest = null,
    Dictionary<string, object>? changes = null)
    {
        var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";

        Dictionary<string, object> logChanges = new Dictionary<string, object>();

        if (changes != null && changes.Count > 0)
        {
            logChanges = changes; // Use only changed values
        }
        else if (newRequest != null)
        {
            logChanges["Edit Request"] = newRequest;
        }

        var auditLog = new AuditLog
        {
            UserEmail = userEmail,
            Action = action.ToString(),
            Entity = entity,
            Timestamp = DateTime.UtcNow,
            Changes = JsonSerializer.Serialize(logChanges, new JsonSerializerOptions { WriteIndented = true })
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }


    public async Task LogLoginAttempt(string email, bool success, string? errorMessage = null)
    {
        var auditLog = new AuditLog
        {
            UserEmail = email,
            Action = success ? "Login Success" : "Login Failed",
            Entity = "Authentication",

            Timestamp = DateTime.UtcNow,
            // Since it's a login, there's no student name, so just store the email
            Changes = success ? "User logged in successfully." : $"Login failed: {errorMessage}"
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Compares old and new data to detect modified fields.
    /// </summary>
    private Dictionary<string, object> GetModifiedFields(StudentFee? oldData, StudentFee newData)
    {
        var changes = new Dictionary<string, object>();

        if (oldData == null) return changes; // No previous data, nothing to compare

        var properties = typeof(StudentFee).GetProperties();
        foreach (var prop in properties)
        {
            var oldValue = prop.GetValue(oldData);
            var newValue = prop.GetValue(newData);

            if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
            {
                changes[prop.Name] = new { Old = oldValue, New = newValue };
            }
        }

        return changes;
    }
}

/// <summary>
/// Enum for standardizing audit actions.
/// </summary>
public enum AuditAction
{
    Created,
    Updated,
    Deleted,
    Approved, // ✅ Add this
    Rejected  // ✅ Add this
}

