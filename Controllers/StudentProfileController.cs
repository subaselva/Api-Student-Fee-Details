using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

namespace StudentFeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentProfileController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Get All StudentProfiles
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudentProfiles()
        {
            var profiles = await _context.StudentProfiles.ToListAsync();
            return Ok(profiles);
        }

        // ✅ Get StudentProfile by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetStudentProfileById(int id)
        {
            var profile = await _context.StudentProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            return Ok(profile);
        }

        // ✅ Add StudentProfile
        //[HttpPost("add")]
        //public async Task<IActionResult> AddStudentProfile([FromBody] StudentProfile profile)
        //{
        //    _context.StudentProfiles.Add(profile);
        //    await _context.SaveChangesAsync();
        //    return Ok(profile);
        //}

        // ✅ Edit StudentProfile
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditStudentProfile(int id, [FromBody] StudentProfile updatedProfile)
        {
            var existing = await _context.StudentProfiles.FindAsync(id);
            if (existing == null) return NotFound();

            UpdateProfileProperties(existing, updatedProfile);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // ✅ Delete StudentProfile
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudentProfile(int id)
        {
            var profile = await _context.StudentProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _context.StudentProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        // 🔧 Helper: Avoid modifying keys
        private void UpdateProfileProperties(StudentProfile existing, StudentProfile updated)
        {
            foreach (var prop in typeof(StudentProfile).GetProperties())
            {
                if (prop.Name == "Id" || prop.Name == "StudentId") continue;

                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }
    }
}
