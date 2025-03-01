using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShelf.Migrations
{
    /// <inheritdoc />
    public partial class MigrationToSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    IdGenre = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.IdGenre);
                });

            migrationBuilder.CreateTable(
                name: "Shelves",
                columns: table => new
                {
                    IdShelf = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 700, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelves", x => x.IdShelf);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    IdBook = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PathImg = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    IdShelf = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 700, nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CountPages = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.IdBook);
                    table.ForeignKey(
                        name: "FK_Books_Shelves_IdShelf",
                        column: x => x.IdShelf,
                        principalTable: "Shelves",
                        principalColumn: "IdShelf",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    IdBookGenre = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdBook = table.Column<int>(type: "INTEGER", nullable: false),
                    IdGenre = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => x.IdBookGenre);
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_IdBook",
                        column: x => x.IdBook,
                        principalTable: "Books",
                        principalColumn: "IdBook",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_IdGenre",
                        column: x => x.IdGenre,
                        principalTable: "Genres",
                        principalColumn: "IdGenre",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReadingSessions",
                columns: table => new
                {
                    IdReadingSession = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReadingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    StartPage = table.Column<int>(type: "INTEGER", nullable: false),
                    FinishPage = table.Column<int>(type: "INTEGER", nullable: false),
                    FinishPercent = table.Column<int>(type: "INTEGER", nullable: false),
                    IdBook = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingSessions", x => x.IdReadingSession);
                    table.ForeignKey(
                        name: "FK_ReadingSessions_Books_IdBook",
                        column: x => x.IdBook,
                        principalTable: "Books",
                        principalColumn: "IdBook",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    IdNote = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdReadingSession = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 700, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.IdNote);
                    table.ForeignKey(
                        name: "FK_Notes_ReadingSessions_IdReadingSession",
                        column: x => x.IdReadingSession,
                        principalTable: "ReadingSessions",
                        principalColumn: "IdReadingSession",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_IdBook",
                table: "BookGenres",
                column: "IdBook");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_IdGenre",
                table: "BookGenres",
                column: "IdGenre");

            migrationBuilder.CreateIndex(
                name: "IX_Books_IdShelf",
                table: "Books",
                column: "IdShelf");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_IdReadingSession",
                table: "Notes",
                column: "IdReadingSession");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingSessions_IdBook",
                table: "ReadingSessions",
                column: "IdBook");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "ReadingSessions");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Shelves");
        }
    }
}
