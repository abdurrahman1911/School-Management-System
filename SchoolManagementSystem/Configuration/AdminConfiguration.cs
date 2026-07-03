using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(a => a.ID);
            builder.Property(a => a.ID).ValueGeneratedOnAdd();
            builder
               .HasOne(a => a.User)
               .WithOne(u => u.Admin)
               .HasForeignKey<Admin>(a => a.UserId)
               .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(a => a.Teachers)
                .WithOne(t => t.Admin)
                .HasForeignKey(t => t.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Supervisors)
                .WithOne(s => s.Admin)
                .HasForeignKey(s => s.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Parents)
                .WithOne(p => p.Admin)
                .HasForeignKey(p => p.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Students)
                .WithOne(s => s.Admin)
                .HasForeignKey(s => s.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.Headmasters)
                .WithOne(h => h.Admin)
                .HasForeignKey(h => h.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
