using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Configuration;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Headmaster> Headmasters { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<StudentExamAnswers> StudentExamAnswers { get; set; }


        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<AcademicTerm> AcademicTerms { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Absence> Absences { get; set; }
        public DbSet<MultiChoiceExam> MultiChoiceExam { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<ExtraSubjectMaterial> ExtraSubjectMaterials { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ClassTimeTable> ClassTimeTable { get; set; }
        public DbSet<TeacherTimeTable> TeacherTimeTables { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Question> Questions { get; set; }


        public DbSet<StudentClassEnrollment> StudentClassEnrollments { get; set; }
        public DbSet<StudentsSubjectsEnrollment> StudentsSubjectsEnrollments { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<StudentExamDegree> StudentExamDegrees { get; set; }
        public DbSet<StudentHomeworkAnswer> StudentHomeworkAnswers { get; set; }
        public DbSet<UserUserType> UserUserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                    typeof(UserConfiguration).Assembly);
        }
    }
}
