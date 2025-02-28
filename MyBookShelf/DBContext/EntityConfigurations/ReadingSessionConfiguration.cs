using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class ReadingSessionConfiguration : IEntityTypeConfiguration<ReadingSession>
    {
        public void Configure(EntityTypeBuilder<ReadingSession> builder)
        {
            builder.HasKey(rs => rs.IdReadingSession);
            builder.Property(rs => rs.IdReadingSession).ValueGeneratedOnAdd();
            builder.Property(rs => rs.ReadingTime).IsRequired();
            builder.Property(rs => rs.StartPage).IsRequired();
            builder.Property(rs => rs.FinishPage).IsRequired();
            builder.Property(rs => rs.FinishPercent);
            builder.HasOne(rs => rs.Book)
                        .WithMany(b => b.ReadingSessions)
                        .HasForeignKey(rs => rs.IdBook)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
