using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n=>n.ID);
            builder.Property(n=>n.ID).ValueGeneratedOnAdd();

            builder
                .HasOne(n => n.WriterUser)
                .WithMany(u => u.WrittenNotes)
                .HasForeignKey(u => u.WriterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.TargetUser)
                .WithMany(u => u.ReceivedNotes)
                .HasForeignKey(u => u.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
