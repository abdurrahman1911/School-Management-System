using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(t=>t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            

        }
    }
}
