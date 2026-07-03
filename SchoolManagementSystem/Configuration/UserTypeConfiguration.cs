using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
       
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.HasKey(t => t.ID);
            builder.Property(t=>t.ID).ValueGeneratedOnAdd();
        }
    }
}
