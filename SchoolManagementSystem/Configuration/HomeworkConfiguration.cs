using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(h=>h.ID);
            builder.Property(h=>h.ID).ValueGeneratedOnAdd();    
            builder
                .HasOne(h=>h.Teacher)
                .WithMany(t=>t.Homeworks)
                .HasForeignKey(h=>h.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.Class)
                .WithMany(c => c.Homeworks)
                .HasForeignKey(c => c.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        
    }
}
