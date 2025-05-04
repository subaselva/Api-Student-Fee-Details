using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

namespace StudentFeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentDetailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentDetailController(AppDbContext context)
        {
            _context = context;
        }


        // ✅ Get All Enrollments
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var enrollments = await _context.StudentEnrollments.ToListAsync();
            return Ok(enrollments);
        }

        // ✅ Get Enrollment by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            var enrollment = await _context.StudentEnrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            return Ok(enrollment);
        }

        // ✅ Add Enrollment
        //[HttpPost("add")]
        //public async Task<IActionResult> AddEnrollment([FromBody] EnrollmentDetail enrollment)
        //{
        //    _context.StudentEnrollments.Add(enrollment);
        //    await _context.SaveChangesAsync();
        //    return Ok(enrollment);
        //}

        // ✅ Edit Enrollment
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditEnrollment(int id, [FromBody] EnrollmentDetail updatedEnrollment)
        {
            var existing = await _context.StudentEnrollments.FindAsync(id);
            if (existing == null) return NotFound();

            UpdateEnrollmentProperties(existing, updatedEnrollment);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

       
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.StudentEnrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            _context.StudentEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

       
        private void UpdateEnrollmentProperties(EnrollmentDetail existing, EnrollmentDetail updated)
        {
            foreach (var prop in typeof(EnrollmentDetail).GetProperties())
            {
                if (prop.Name == "Id" || prop.Name == "StudentId") continue;

                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }
    }
}
