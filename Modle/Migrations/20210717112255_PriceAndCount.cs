using Microsoft.EntityFrameworkCore.Migrations;

namespace Modle.Migrations
{
    public partial class PriceAndCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PriceInIQD",
                table: "Order",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceInUSD",
                table: "Order",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PriceInIQD",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PriceInUSD",
                table: "Order");
        }
    }
}
