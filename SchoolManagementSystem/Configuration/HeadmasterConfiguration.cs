using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class HeadmasterConfiguration : IEntityTypeConfiguration<Headmaster>
    {
        public void Configure(EntityTypeBuilder<Headmaster> builder)
        {
            builder.HasKey(h=>h.ID);
            builder.Property(h => h.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(h => h.User)
                .WithOne(u => u.Headmaster)
                .HasForeignKey<Headmaster>(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
