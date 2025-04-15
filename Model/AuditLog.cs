using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace StudentFeeManagement.Model
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserEmail { get; set; } // User who performed the action

        [Required]
        public string Action { get; set; } // Create, Update, Delete

        [Required]
        public string Entity { get; set; } // Table affected (e.g., "StudentFees")

        

        public string Changes { get; set; } // JSON serialized object with modified fields

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
