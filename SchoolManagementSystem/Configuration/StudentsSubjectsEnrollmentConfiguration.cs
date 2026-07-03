using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class StudentsSubjectsEnrollmentConfiguration : IEntityTypeConfiguration<StudentsSubjectsEnrollment>
    {
        public void Configure(EntityTypeBuilder<StudentsSubjectsEnrollment> builder)
        {
            builder.HasKey(a => a.ID);
            builder.Property(a => a.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(s=>s.Subject)
                .WithMany(s=>s.StudentsSubjectsEnrollments)
                .HasForeignKey(s => s.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.Teacher)
                .WithMany(s => s.StudentsSubjectsEnrollments)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.AcademicTerm)
                .WithMany(s => s.StudentsSubjectsEnrollments)
                .HasForeignKey(s => s.AcademicTermId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.Student)
                .WithMany(s => s.StudentsSubjectsEnrollments)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
