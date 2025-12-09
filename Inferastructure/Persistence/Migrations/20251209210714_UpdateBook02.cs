using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBook02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows",
                column: "BookId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows",
                column: "BookId1",
                unique: true,
                filter: "[BookId1] IS NOT NULL");
        }
    }
}
