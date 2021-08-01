using Microsoft.EntityFrameworkCore.Migrations;

namespace Modle.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISInProgress",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.AddColumn<bool>(
                name: "ISInProgress",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
