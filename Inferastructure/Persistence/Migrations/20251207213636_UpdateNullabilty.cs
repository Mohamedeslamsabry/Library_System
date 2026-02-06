using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNullabilty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Shelves_ShelfId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Floors_FloorNumber",
                table: "Shelves");

            migrationBuilder.AlterColumn<int>(
                name: "FloorNumber",
                table: "Shelves",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShelfId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Shelves_ShelfId",
                table: "Books",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Floors_FloorNumber",
                table: "Shelves",
                column: "FloorNumber",
                principalTable: "Floors",
                principalColumn: "FloorNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Shelves_ShelfId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Shelves_Floors_FloorNumber",
                table: "Shelves");

            migrationBuilder.AlterColumn<int>(
                name: "FloorNumber",
                table: "Shelves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShelfId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Shelves_ShelfId",
                table: "Books",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shelves_Floors_FloorNumber",
                table: "Shelves",
                column: "FloorNumber",
                principalTable: "Floors",
                principalColumn: "FloorNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
