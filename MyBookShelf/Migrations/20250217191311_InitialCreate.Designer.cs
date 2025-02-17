﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBookShelf.DBContext;

#nullable disable

namespace MyBookShelf.Migrations
{
    [DbContext(typeof(BookShelfDBContext))]
    [Migration("20250217191311_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyBookShelf.Models.Book", b =>
                {
                    b.Property<int>("IdBook")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBook"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CountPages")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.Property<int>("IdShelf")
                        .HasColumnType("int");

                    b.Property<string>("PathImg")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdBook");

                    b.HasIndex("IdShelf");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("MyBookShelf.Models.BookGenre", b =>
                {
                    b.Property<int>("IdBookGenre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBookGenre"));

                    b.Property<int>("IdBook")
                        .HasColumnType("int");

                    b.Property<int>("IdGenre")
                        .HasColumnType("int");

                    b.HasKey("IdBookGenre");

                    b.HasIndex("IdBook");

                    b.HasIndex("IdGenre");

                    b.ToTable("BookGenres");
                });

            modelBuilder.Entity("MyBookShelf.Models.Genre", b =>
                {
                    b.Property<int>("IdGenre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGenre"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdGenre");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MyBookShelf.Models.Note", b =>
                {
                    b.Property<int>("IdNote")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdNote"));

                    b.Property<int>("IdReadingSession")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.HasKey("IdNote");

                    b.HasIndex("IdReadingSession");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("MyBookShelf.Models.ReadingSession", b =>
                {
                    b.Property<int>("IdReadingSession")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReadingSession"));

                    b.Property<int>("FinishPage")
                        .HasColumnType("int");

                    b.Property<int>("FinishPercent")
                        .HasColumnType("int");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdBook")
                        .HasColumnType("int");

                    b.Property<int>("StartPage")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("IdReadingSession");

                    b.HasIndex("IdBook");

                    b.ToTable("ReadingSessions");
                });

            modelBuilder.Entity("MyBookShelf.Models.Shelf", b =>
                {
                    b.Property<int>("IdShelf")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdShelf"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("IdShelf");

                    b.ToTable("Shelves");
                });

            modelBuilder.Entity("MyBookShelf.Models.Book", b =>
                {
                    b.HasOne("MyBookShelf.Models.Shelf", "Shelf")
                        .WithMany("Books")
                        .HasForeignKey("IdShelf")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Shelf");
                });

            modelBuilder.Entity("MyBookShelf.Models.BookGenre", b =>
                {
                    b.HasOne("MyBookShelf.Models.Book", "Book")
                        .WithMany("BookGenres")
                        .HasForeignKey("IdBook")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyBookShelf.Models.Genre", "Genre")
                        .WithMany("BookGenres")
                        .HasForeignKey("IdGenre")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("MyBookShelf.Models.Note", b =>
                {
                    b.HasOne("MyBookShelf.Models.ReadingSession", "ReadingSession")
                        .WithMany("Notes")
                        .HasForeignKey("IdReadingSession")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ReadingSession");
                });

            modelBuilder.Entity("MyBookShelf.Models.ReadingSession", b =>
                {
                    b.HasOne("MyBookShelf.Models.Book", "Book")
                        .WithMany("ReadingSessions")
                        .HasForeignKey("IdBook")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("MyBookShelf.Models.Book", b =>
                {
                    b.Navigation("BookGenres");

                    b.Navigation("ReadingSessions");
                });

            modelBuilder.Entity("MyBookShelf.Models.Genre", b =>
                {
                    b.Navigation("BookGenres");
                });

            modelBuilder.Entity("MyBookShelf.Models.ReadingSession", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("MyBookShelf.Models.Shelf", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
