using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(bg => bg.IdBookGenre);
            builder.HasOne(bg => bg.Book)
                    .WithMany(b => b.BookGenres)
                    .HasForeignKey(bg => bg.IdBook)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            builder.HasOne(bg => bg.Genre)
                   .WithMany(g => g.BookGenres) 
                   .HasForeignKey(bg => bg.IdGenre)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();
        }
    }

}
