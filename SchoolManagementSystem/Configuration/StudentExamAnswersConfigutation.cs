using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{

    public class StudentExamAnswersConfigutation : IEntityTypeConfiguration<StudentExamAnswers>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StudentExamAnswers> builder)
        {
            builder.Property(s=>s.ID).ValueGeneratedOnAdd();
            builder.HasOne(s => s.Question)
                .WithMany(q => q.StudentExamAnswers)
                .HasForeignKey(s => s.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.QuestionAnswer)
                .WithMany(q => q.StudentExamAnswers)
                .HasForeignKey(s => s.StudentAnswersID)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.StudentExamDegree)
                .WithMany(s => s.StudentExamAnswers)
                .HasForeignKey(s => s.StudentExamDegreeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
