using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BorrowModuels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Borrows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookId1",
                table: "Borrows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Borrows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Borrows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows",
                column: "BookId1",
                unique: true,
                filter: "[BookId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_EmployeeId1",
                table: "Borrows",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Books_BookId1",
                table: "Borrows",
                column: "BookId1",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Employees_EmployeeId1",
                table: "Borrows",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Books_BookId1",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Employees_EmployeeId1",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId1",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_EmployeeId1",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "BookId1",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Borrows");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
