using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sln.Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditColumnFinishedOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "FinishedOrders",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FinishedOrders",
                newName: "User");
        }
    }
}
