using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sln.Estoque.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingMinimumQuantityProductColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FinishedOrders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MinimumQuantity",
                table: "Products",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumQuantity",
                table: "Products");

            migrationBuilder.InsertData(
                table: "FinishedOrders",
                columns: new[] { "Id", "DateFinish", "LayoutCode", "OrderId", "Quantity", "User" },
                values: new object[] { 1, null, 100, 46693, 350, 1 });
        }
    }
}
