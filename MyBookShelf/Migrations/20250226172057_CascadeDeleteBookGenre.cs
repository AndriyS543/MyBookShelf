using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShelf.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteBookGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_IdBook",
                table: "BookGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_IdBook",
                table: "BookGenres",
                column: "IdBook",
                principalTable: "Books",
                principalColumn: "IdBook",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_IdBook",
                table: "BookGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_IdBook",
                table: "BookGenres",
                column: "IdBook",
                principalTable: "Books",
                principalColumn: "IdBook",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
