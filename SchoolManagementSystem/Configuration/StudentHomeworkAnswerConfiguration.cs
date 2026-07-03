using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class StudentHomeworkAnswerConfiguration : IEntityTypeConfiguration<StudentHomeworkAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentHomeworkAnswer> builder)
        {
            builder.HasKey(h => h.ID);
            builder.Property(h => h.ID).ValueGeneratedOnAdd();
            builder
                .HasOne(h => h.Homework)
                .WithMany(t => t.StudentHomeworkAnswers)
                .HasForeignKey(h => h.HomeworkId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.Student)
                .WithMany(t => t.StudentHomeworkAnswers)
                .HasForeignKey(h => h.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
