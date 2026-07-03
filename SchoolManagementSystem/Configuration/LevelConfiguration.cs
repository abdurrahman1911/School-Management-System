using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasKey(g=>g.ID);
            builder.Property(g=>g.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(g=>g.Stage)
                .WithMany(l=>l.Levels)
                .HasForeignKey(l=>l.StageID)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
