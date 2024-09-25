using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infra.EF.Migrations
{
    /// <inheritdoc />
    public partial class basket_add_price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Basket",
                type: "decimal(18,2)",                
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Basket");
        }
    }
}
