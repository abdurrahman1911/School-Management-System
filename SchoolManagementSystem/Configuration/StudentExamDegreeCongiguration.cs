using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{

    public class StudentExamDegreeCongiguration : IEntityTypeConfiguration<StudentExamDegree>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StudentExamDegree> builder)
        {
            builder.HasKey(s=>s.ID);
            builder.Property(s=>s.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(s => s.Exam)
                .WithMany(e => e.StudentExamDegrees)
                .HasForeignKey(s => s.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(s => s.Student)
                .WithMany(s => s.StudentExamDegrees)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Property(d => d.Degree)
                .HasPrecision(5, 2);

        }
    }
}
