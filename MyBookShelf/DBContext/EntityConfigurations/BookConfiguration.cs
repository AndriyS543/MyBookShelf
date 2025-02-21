﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext.EntityConfigurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.IdBook);
            builder.Property(b => b.Title).HasMaxLength(255).IsRequired();
            builder.Property(b => b.PublicationDate).IsRequired();
            builder.Property(b => b.PathImg).HasMaxLength(255);
            builder.Property(b => b.Rating);
            builder.Property(b => b.Description).HasMaxLength(700);
            builder.Property(b => b.Author).HasMaxLength(50);
            builder.Property(b => b.CountPages);
            builder.HasOne(b => b.Shelf)
                        .WithMany(s => s.Books)
                        .HasForeignKey(b => b.IdShelf)
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
