using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(n => n.IdNote);
            builder.Property(n => n.Text).HasMaxLength(700).IsRequired();
            builder.HasOne<ReadingSession>()
                   .WithMany()
                   .HasForeignKey(n => n.IdReadingSession);
        }
    }
}
