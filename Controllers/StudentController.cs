using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml; // For Excel Export
using iText.Kernel.Pdf; // Using iText7
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Service;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
using IronXL;
using ClosedXML.Excel;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
namespace StudentFeeManagement.Controllers
{
   

    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly AppDbContext _context;
        private readonly AuditService _auditService;

        public StudentController(AppDbContext context, StudentService studentService, AuditService auditService)
        {
            _context = context;
            _studentService = studentService;
            _auditService = auditService;
        }


        [HttpPost("full/add")]
        public async Task<IActionResult> AddFullStudent([FromBody] FullStudentInfo full)
        {
            if (full?.Student == null)
                return BadRequest("Invalid student data");

            _context.Students.Add(full.Student);
            await _context.SaveChangesAsync();
           
            // ✅ Log creation
            await _auditService.LogAsync("Create", "Student", full.Student);

            // 🛡️ Audit log for creating a new student

            return Ok(full.Student);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFullStudent(int id, [FromBody] FullStudentInfo fullInfo)
        {
            if (fullInfo?.Student == null) return BadRequest("Invalid data");

            var existingStudent = await _context.Students
                .Include(s => s.Enrollment)
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null) return NotFound("Student not found");

            // Update student (exclude ID)
            fullInfo.Student.Id = id; // ensure it matches
            _context.Entry(existingStudent).CurrentValues.SetValues(fullInfo.Student);

            // Update enrollment if exists
            if (existingStudent.Enrollment != null && fullInfo.Student.Enrollment != null)
            {
                fullInfo.Student.Enrollment.Id = existingStudent.Enrollment.Id; // keep original ID
                fullInfo.Student.Enrollment.StudentId = id;
                _context.Entry(existingStudent.Enrollment).CurrentValues.SetValues(fullInfo.Student.Enrollment);
            }

            // Update profile if exists
            if (existingStudent.Profile != null && fullInfo.Student.Profile != null)
            {
                fullInfo.Student.Profile.Id = existingStudent.Profile.Id; // keep original ID
                fullInfo.Student.Profile.StudentId = id;
                _context.Entry(existingStudent.Profile).CurrentValues.SetValues(fullInfo.Student.Profile);
            }

            await _context.SaveChangesAsync();

            await _auditService.LogAsync("Update", "Student", new
            {
                Student = fullInfo.Student,
                Enrollment = fullInfo.Student.Enrollment,
                Profile = fullInfo.Student.Profile
            });

            return Ok("Updated successfully");
        }

        // 🔹 Admin - Delete Student
        [HttpDelete("admin/delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            
            var studentToDelete = await _studentService.GetStudentById(id);
            var isDeleted = await _studentService.DeleteStudent(id);
            if (isDeleted && studentToDelete != null)
                await _auditService.LogAsync("Delete", "Students", studentToDelete);
            return isDeleted ? Ok(new { message = "Student deleted successfully" }) : NotFound(new { message = "Student not found" });
        }

        // GET full student view by ID
        [HttpGet("fullview/{id}")]
        public async Task<IActionResult> GetFullStudentInfo(int id)
        {
            var fullInfo = await _studentService.GetFullStudentDetails(id);
            if (fullInfo == null) return NotFound();
            return Ok(fullInfo);
        }

        [HttpGet("fullview")]
        public async Task<IActionResult> GetAllStudentInfo()
        {
            var fullInfo = await _studentService.GetAllStudentDetailsAsync();

            if (fullInfo == null || !fullInfo.Any())
                return NotFound("No student data found.");

            return Ok(fullInfo);  // Return all student details including Enrollment and Profile
        }


        [HttpGet("view-only")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);
        }

        [HttpGet("view-only/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentService.GetStudentById(id);
            return student == null ? NotFound() : Ok(student);
        }


        

        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            var students = _context.Students.AsNoTracking().ToList();
            if (!students.Any())
                return BadRequest("No students found to export.");

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Students");

            var props = typeof(Student).GetProperties();

            // Headers
            for (int col = 0; col < props.Length; col++)
            {
                sheet.Cell(1, col + 1).Value = props[col].Name;
                sheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            // Rows
            for (int row = 0; row < students.Count; row++)
            {
                var student = students[row];
                for (int col = 0; col < props.Length; col++)
                {
                    var value = props[col].GetValue(student);
                    sheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                }
            }

            sheet.Columns().AdjustToContents(); // auto fit

            // 🔥 DO NOT use 'using' here
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Students.xlsx");
        }



       
    }

}
