using Microsoft.EntityFrameworkCore;
using StudentFeeManagement.Data;
using StudentFeeManagement.Model;

namespace StudentFeeManagement.Service
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student> AddStudent(Student student)
        {
            student.RollNumber = GenerateRollNumber();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> EditStudent(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null) return null;

            UpdateStudentProperties(existingStudent, student);
            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }


        private void UpdateStudentProperties(Student existing, Student updated)
        {
            foreach (var prop in typeof(Student).GetProperties())
            {
                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }

        private string GenerateRollNumber()
        {
            return "ROLL-" + DateTime.UtcNow.Ticks.ToString().Substring(8);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
