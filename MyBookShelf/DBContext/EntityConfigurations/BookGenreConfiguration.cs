using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(bg => new { bg.IdBook, bg.IdGenre });
            builder.HasOne<Book>()
                   .WithMany()
                   .HasForeignKey(bg => bg.IdBook);
            builder.HasOne<Genre>()
                   .WithMany()
                   .HasForeignKey(bg => bg.IdGenre);
        }
    }

}
