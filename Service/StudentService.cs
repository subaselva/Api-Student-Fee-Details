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


        public async Task<FullStudentInfo?> GetFullStudentDetails(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Enrollment)
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return null;

            return new FullStudentInfo
            {
                Student = student,
                
            };
        }

        public async Task<List<FullStudentInfo>> GetAllStudentDetailsAsync()
        {
            var students = await _context.Students
                .Include(s => s.Enrollment)  // Include Enrollment data
                .Include(s => s.Profile)     // Include Profile data
                .ToListAsync();

            // Map the data to FullStudentInfo model
            var fullStudentInfos = students.Select(student => new FullStudentInfo
            {
                Student = student,
            }).ToList();

            return fullStudentInfos;
        }



        public async Task<Student> EditStudentAsync(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null) return null;

            UpdateStudentProperties(existingStudent, student);
            await _context.SaveChangesAsync();
            return existingStudent;
        }

        private void UpdateStudentProperties(Student existing, Student updated)
        {
            foreach (var prop in typeof(Student).GetProperties())
            {
                if (prop.Name == "StudentId") continue; // Skip the primary key

                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EnrollmentDetail>> GetEnrollments()
        {
            return await _context.StudentEnrollments.AsNoTracking().ToListAsync();
        }

       
       

        public async Task<EnrollmentDetail> EditEnrollment(int id, EnrollmentDetail enrollment)
        {
            var existingEnrollment = await _context.StudentEnrollments.FindAsync(id);
            if (existingEnrollment == null) return null;

            UpdateEnrollmentProperties(existingEnrollment, enrollment);
            await _context.SaveChangesAsync();
            return existingEnrollment;
        }

        public async Task<bool> DeleteEnrollment(int id)
        {
            var enrollment = await _context.StudentEnrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.StudentEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StudentProfile>> GetProfiles()
        {
            return await _context.StudentProfiles.AsNoTracking().ToListAsync();
        }

        

        public async Task<StudentProfile> EditProfile(int id, StudentProfile profile)
        {
            var existingProfile = await _context.StudentProfiles.FindAsync(id);
            if (existingProfile == null) return null;

            UpdateProfileProperties(existingProfile, profile);
            await _context.SaveChangesAsync();
            return existingProfile;
        }

        public async Task<bool> DeleteProfile(int id)
        {
            var profile = await _context.StudentProfiles.FindAsync(id);
            if (profile == null) return false;

            _context.StudentProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
       

        private void UpdateEnrollmentProperties(EnrollmentDetail existing, EnrollmentDetail updated)
        {
            foreach (var prop in typeof(EnrollmentDetail).GetProperties())
            {
                if (prop.Name == "Id") continue; // Skip the primary key

                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }

        private void UpdateProfileProperties(StudentProfile existing, StudentProfile updated)
        {
            foreach (var prop in typeof(StudentProfile).GetProperties())
            {
                if (prop.Name == "Id") continue; // Skip the primary key

                var value = prop.GetValue(updated);
                if (value != null)
                    prop.SetValue(existing, value);
            }
        }
        public async Task<Student?> GetStudentById(int id)
        {
            
            return await _context.Students.FindAsync(id);
        }

        public async Task<EnrollmentDetail?> GetEnrollmentById(int id)
        {

            return await _context.StudentEnrollments.FindAsync(id);
        }

        public async Task<StudentProfile?> GetProfileById(int id)
        {

            return await _context.StudentProfiles.FindAsync(id);
        }
        //private string GenerateRollNumber()
        //{
        //    return "ROLL-" + DateTime.UtcNow.Ticks.ToString().Substring(8);
        //}

        

    }

}
