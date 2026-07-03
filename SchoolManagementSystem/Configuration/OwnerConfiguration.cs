using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(u => u.ID);
            builder.Property(u => u.ID).ValueGeneratedOnAdd();
            builder
               .HasOne(a => a.User)
               .WithOne(u => u.Owner)
               .HasForeignKey<Owner>(a => a.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
