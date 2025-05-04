using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;
using System.IO;

namespace StudentFeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExportController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Export to Excel with 3 classes: Student, EnrollmentDetail, StudentProfile
        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            // Fetch data from the database
            var students = _context.Students.AsNoTracking().ToList();
            var enrollments = _context.StudentEnrollments.AsNoTracking().ToList();
            var profiles = _context.StudentProfiles.AsNoTracking().ToList();

            if (!students.Any() && !enrollments.Any() && !profiles.Any())
                return BadRequest("No data found to export.");

            // Create a new Excel workbook
            var workbook = new XLWorkbook();

            // Export Students
            var studentSheet = workbook.Worksheets.Add("Students");
            ExportToSheet(studentSheet, students, typeof(Student));

            // Export Enrollments
            var enrollmentSheet = workbook.Worksheets.Add("Enrollments");
            ExportToSheet(enrollmentSheet, enrollments, typeof(EnrollmentDetail));

            // Export Profiles
            var profileSheet = workbook.Worksheets.Add("Profiles");
            ExportToSheet(profileSheet, profiles, typeof(StudentProfile));

            // Auto-adjust columns for all sheets
            foreach (var sheet in workbook.Worksheets)
            {
                sheet.Columns().AdjustToContents();
            }

            // Prepare stream for download
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ExportedData.xlsx");
        }

        // Helper method to export data to an Excel sheet
        private void ExportToSheet<T>(IXLWorksheet sheet, List<T> data, Type type)
        {
            var properties = type.GetProperties();

            // Headers
            for (int col = 0; col < properties.Length; col++)
            {
                sheet.Cell(1, col + 1).Value = properties[col].Name;
                sheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            // Rows
            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item);
                    sheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                }
            }
        }
    }
}
