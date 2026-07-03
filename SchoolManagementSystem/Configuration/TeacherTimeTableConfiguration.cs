using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class TeacherTimeTableConfiguration : IEntityTypeConfiguration<TeacherTimeTable>
    {
        public void Configure(EntityTypeBuilder<TeacherTimeTable> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder
                .HasOne(t => t.Teacher)
                .WithOne(u => u.TeacherTimeTable)
                .HasForeignKey<TeacherTimeTable>(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
