using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class ClassTimeTableConfiguration : IEntityTypeConfiguration<ClassTimeTable>
    {
        public void Configure(EntityTypeBuilder<ClassTimeTable> builder)
        {
            builder.HasKey(t=>t.ID);
            builder.Property(t=>t.ID).ValueGeneratedOnAdd();

            

            builder
                .HasOne(t => t.Class)
                .WithOne(t => t.ClassTimeTable)
                .HasForeignKey<ClassTimeTable>(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);





        }
    }
}
