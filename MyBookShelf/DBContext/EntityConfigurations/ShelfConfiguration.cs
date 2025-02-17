using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> builder)
        {
            builder.HasKey(s => s.IdShelf);
            builder.Property(s => s.Name).HasMaxLength(255).IsRequired();
            builder.Property(s => s.Description).HasMaxLength(700);
        }
    }
}
