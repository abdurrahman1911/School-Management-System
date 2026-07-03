using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class ExtraSubjectMaterialConfiguration : IEntityTypeConfiguration<ExtraSubjectMaterial>
    {
        public void Configure(EntityTypeBuilder<ExtraSubjectMaterial> builder)
        {

            builder.HasKey(s => s.ID);
            builder.Property(s => s.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(e=>e.Subject)
                .WithMany(s=>s.ExtraSubjectMaterials)
                .HasForeignKey(s => s.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e => e.Class)
                .WithMany(s => s.ExtraSubjectMaterials)
                .HasForeignKey(s => s.ClassID)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e=>e.Teacher)
                .WithMany(t=>t.ExtraSubjectsMaterials)
                .HasForeignKey(e=>e.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
