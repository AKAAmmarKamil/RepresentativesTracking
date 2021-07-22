using Microsoft.EntityFrameworkCore.Migrations;

namespace Modle.Migrations
{
    public partial class Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PriceInIQD = table.Column<double>(type: "float", nullable: true),
                    PriceInUSD = table.Column<double>(type: "float", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderID",
                table: "Products",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

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
    }
}
