using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class StudentClassEnrollmentConfiguration : IEntityTypeConfiguration<StudentClassEnrollment>
    {
        public void Configure(EntityTypeBuilder<StudentClassEnrollment> builder)
        {
            builder.HasKey(s=>s.ID);
            builder.Property(s => s.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(s=>s.Student)
                .WithMany(s=>s.StudentClassEnrollments)
                .HasForeignKey(s=>s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.Level)
                .WithMany(s => s.StudentClassEnrollments)
                .HasForeignKey(s => s.LevelID)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(s => s.AcademicTerm)
                .WithMany(s => s.StudentClassEnrollments)
                .HasForeignKey(s => s.AcademicTermId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasOne(s => s.Class)
               .WithMany(s => s.StudentClassEnrollments)
               .HasForeignKey(s => s.ClassId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
