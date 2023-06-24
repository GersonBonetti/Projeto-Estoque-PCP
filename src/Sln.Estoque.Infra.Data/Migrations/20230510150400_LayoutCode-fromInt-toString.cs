using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sln.Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class LayoutCodefromInttoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LayoutCode",
                table: "FinishedOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedOrders_UserId",
                table: "FinishedOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinishedOrders_Users_UserId",
                table: "FinishedOrders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinishedOrders_Users_UserId",
                table: "FinishedOrders");

            migrationBuilder.DropIndex(
                name: "IX_FinishedOrders_UserId",
                table: "FinishedOrders");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutCode",
                table: "FinishedOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
