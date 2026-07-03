using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class SupervisorConfiguration : IEntityTypeConfiguration<Supervisor>
    {
        public void Configure(EntityTypeBuilder<Supervisor> builder)
        {
            builder.HasKey(s => s.ID);
            builder.Property(s => s.ID).ValueGeneratedOnAdd();

            builder
               .HasOne(s => s.User)
               .WithOne(u => u.Supervisor)
               .HasForeignKey<Supervisor>(s => s.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
