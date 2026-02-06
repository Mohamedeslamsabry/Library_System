using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CheckDate",
                table: "Borrows");

            migrationBuilder.AddCheckConstraint(
                name: "CheckDate",
                table: "Borrows",
                sql: "DateBorrow < DueDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CheckDate",
                table: "Borrows");

            migrationBuilder.AddCheckConstraint(
                name: "CheckDate",
                table: "Borrows",
                sql: "DateBorrow > DueDate");
        }
    }
}
