using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class UserUserTypeConfiguration : IEntityTypeConfiguration<UserUserType>
    {
        public void Configure(EntityTypeBuilder<UserUserType> builder)
        {
            builder.HasKey(u=>u.ID);
            builder.Property(u => u.ID).ValueGeneratedOnAdd();
            builder
                .HasOne(u => u.User)
                .WithMany(u => u.UserUserTypes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
               .HasOne(u => u.UserType)
               .WithMany(u => u.UserUserTypes)
               .HasForeignKey(u => u.UserTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        }

       
    }
}
