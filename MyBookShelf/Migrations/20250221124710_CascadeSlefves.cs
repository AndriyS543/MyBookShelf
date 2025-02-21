using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShelf.Migrations
{
    /// <inheritdoc />
    public partial class CascadeSlefves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Shelves_IdShelf",
                table: "Books");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Shelves_IdShelf",
                table: "Books",
                column: "IdShelf",
                principalTable: "Shelves",
                principalColumn: "IdShelf",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Shelves_IdShelf",
                table: "Books");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Shelves_IdShelf",
                table: "Books",
                column: "IdShelf",
                principalTable: "Shelves",
                principalColumn: "IdShelf");
        }
    }
}
