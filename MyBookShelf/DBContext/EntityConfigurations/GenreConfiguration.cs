using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(g => g.IdGenre);
            builder.Property(g => g.Name).HasMaxLength(255).IsRequired();
            builder.HasIndex(g => g.Name).IsUnique();
        }
    }

}
