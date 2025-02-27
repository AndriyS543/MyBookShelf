using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShelf.Migrations
{
    /// <inheritdoc />
    public partial class Modify_ReadingSession_Time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "ReadingSessions");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ReadingSessions",
                newName: "ReadingTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReadingTime",
                table: "ReadingSessions",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishTime",
                table: "ReadingSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
