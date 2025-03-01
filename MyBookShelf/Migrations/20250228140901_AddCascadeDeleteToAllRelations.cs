using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShelf.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToAllRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_IdGenre",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_ReadingSessions_IdReadingSession",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingSessions_Books_IdBook",
                table: "ReadingSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_IdGenre",
                table: "BookGenres",
                column: "IdGenre",
                principalTable: "Genres",
                principalColumn: "IdGenre",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_ReadingSessions_IdReadingSession",
                table: "Notes",
                column: "IdReadingSession",
                principalTable: "ReadingSessions",
                principalColumn: "IdReadingSession",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingSessions_Books_IdBook",
                table: "ReadingSessions",
                column: "IdBook",
                principalTable: "Books",
                principalColumn: "IdBook",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Genres_IdGenre",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_ReadingSessions_IdReadingSession",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingSessions_Books_IdBook",
                table: "ReadingSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Genres_IdGenre",
                table: "BookGenres",
                column: "IdGenre",
                principalTable: "Genres",
                principalColumn: "IdGenre",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_ReadingSessions_IdReadingSession",
                table: "Notes",
                column: "IdReadingSession",
                principalTable: "ReadingSessions",
                principalColumn: "IdReadingSession",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingSessions_Books_IdBook",
                table: "ReadingSessions",
                column: "IdBook",
                principalTable: "Books",
                principalColumn: "IdBook",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
