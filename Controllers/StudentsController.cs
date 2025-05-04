using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

namespace StudentFeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Add Student
        //[HttpPost("add")]
        //public async Task<IActionResult> AddStudent([FromBody] Student student)
        //{
        //    _context.Students.Add(student);
        //    await _context.SaveChangesAsync();
        //    return Ok(student);
        //}

        // ✅ Edit Student
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditStudent(int id, [FromBody] Student updatedStudent)
        {
            var existing = await _context.Students.FindAsync(id);
            if (existing == null) return NotFound();

            // Update the student properties
            existing.Name = updatedStudent.Name;
            existing.Gender = updatedStudent.Gender;
            existing.DateOfBirth = updatedStudent.DateOfBirth;
            existing.PermanentEducationNumber = updatedStudent.PermanentEducationNumber;
            existing.MotherName = updatedStudent.MotherName;
            existing.FatherName = updatedStudent.FatherName;
            existing.GuardianName = updatedStudent.GuardianName;
            existing.AadharNumber = updatedStudent.AadharNumber;
            existing.NameOnAadhar = updatedStudent.NameOnAadhar;
            existing.Address = updatedStudent.Address;
            existing.Pincode = updatedStudent.Pincode;
            existing.MobileNumber = updatedStudent.MobileNumber;
            existing.AlternateMobileNumber = updatedStudent.AlternateMobileNumber;
            existing.Email = updatedStudent.Email;
            existing.MotherTongue = updatedStudent.MotherTongue;
            existing.SocialCategory = updatedStudent.SocialCategory;
            existing.MinorityGroup = updatedStudent.MinorityGroup;
            existing.IsBPLBeneficiary = updatedStudent.IsBPLBeneficiary;
            existing.IsAAYBeneficiary = updatedStudent.IsAAYBeneficiary;
            existing.IsEWS = updatedStudent.IsEWS;
            existing.IsCWSN = updatedStudent.IsCWSN;
            existing.TypeOfImpairment = updatedStudent.TypeOfImpairment;
            existing.IsIndian = updatedStudent.IsIndian;
            existing.IsOutOfSchool = updatedStudent.IsOutOfSchool;
            existing.HasDisabilityCertificate = updatedStudent.HasDisabilityCertificate;
            existing.DisabilityPercentage = updatedStudent.DisabilityPercentage;
            existing.BloodGroup = updatedStudent.BloodGroup;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // ✅ Delete Student
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        // ✅ Get All Students
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        // ✅ Get Student by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return Ok(student);
        }
    }
}
