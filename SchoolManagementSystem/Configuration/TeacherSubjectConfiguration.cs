using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
    {
        public void Configure(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.HasKey(u => u.ID);
            builder.Property(u => u.ID).ValueGeneratedOnAdd();
            builder
                .HasOne(t=>t.Subject)
                .WithMany(t=>t.TeacherSubjects)
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.Restrict); 
            builder
                .HasOne(t => t.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(t => t.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
