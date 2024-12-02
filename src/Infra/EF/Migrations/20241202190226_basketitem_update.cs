using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infra.EF.Migrations
{
    /// <inheritdoc />
    public partial class basketitem_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "BasketItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BasketItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "BasketItem",
                type: "nvarchar(90)",
                nullable: false,
                defaultValue: "");
        }
    }
}
