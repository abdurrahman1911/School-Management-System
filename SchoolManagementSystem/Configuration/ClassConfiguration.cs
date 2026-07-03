using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c=>c.ID);
            builder.Property(c=>c.ID).ValueGeneratedOnAdd();


            builder
                .HasOne(c=>c.Level)
                .WithMany(g=>g.Classes)
                .HasForeignKey(c=>c.LevelID)
                .OnDelete(DeleteBehavior.Restrict);
            
           


        }
    }

   
}
