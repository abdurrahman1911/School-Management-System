using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
    {
        public void Configure(EntityTypeBuilder<Absence> builder)
        {
            builder.HasKey(a=>a.ID);
            builder.Property(a=>a.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(a => a.User)
                .WithMany(u => u.Absences)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
