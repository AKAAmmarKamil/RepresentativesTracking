using Microsoft.EntityFrameworkCore.Migrations;

namespace Modle.Migrations
{
    public partial class IsInProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ISInProgress",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISInProgress",
                table: "Order");
        }
    }
}
