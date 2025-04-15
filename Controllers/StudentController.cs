using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml; // For Excel Export
using iText.Kernel.Pdf; // Using iText7
using iText.Layout;
using iText.Layout.Element;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Service;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
namespace StudentFeeManagement.Controllers
{
   

    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context, StudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        // 🔹 Admin - Add Student
        [HttpPost("admin/add")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            var addedStudent = await _studentService.AddStudent(student);
            return Ok(addedStudent);
        }

        // 🔹 Admin - Edit Student
        [HttpPut("admin/edit/{id}")]
        public async Task<IActionResult> EditStudent(int id, [FromBody] Student student)
        {
            var updatedStudent = await _studentService.EditStudent(id, student);
            return updatedStudent == null ? NotFound() : Ok(updatedStudent);
        }
        // 🔹 Admin - Delete Student
        [HttpDelete("admin/delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var isDeleted = await _studentService.DeleteStudent(id);
            return isDeleted ? Ok(new { message = "Student deleted successfully" }) : NotFound(new { message = "Student not found" });
        }

        // 🔹 View-Only Student List
        [HttpGet("view-only")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);
        }

        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            // Set License Context explicitly in case it's not set
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Students");

            // Fetch students from the database
            var students = _context.Students.AsNoTracking().ToList();

            if (students.Count == 0)
            {
                return BadRequest("No students found to export.");
            }

            // Load data into Excel sheet
            worksheet.Cells[1, 1].LoadFromCollection(students, true);

            // Save to memory stream
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
        }





        // 🔹 Export to PDF (Using iText7)
        [HttpGet("export/pdf")]
        public IActionResult ExportToPDF()
        {
            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var students = _context.Students.AsNoTracking().ToList();
            foreach (var student in students)
            {
                document.Add(new Paragraph($"Roll No: {student.RollNumber} - Name: {student.Name}"));
            }

            document.Close();
            stream.Position = 0;

            return File(stream, "application/pdf", "Students.pdf");
        }
    }

}
