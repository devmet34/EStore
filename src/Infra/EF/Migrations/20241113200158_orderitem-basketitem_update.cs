using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infra.EF.Migrations
{
    /// <inheritdoc />
    public partial class orderitembasketitem_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderItem",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItem",
                type: "nvarchar(90)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "BasketItem",
                type: "nvarchar(90)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "BasketItem");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItem",
                newName: "TotalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Products_ProductId",
                table: "OrderItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
