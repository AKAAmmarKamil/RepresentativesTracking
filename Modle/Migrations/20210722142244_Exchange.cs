using Microsoft.EntityFrameworkCore.Migrations;

namespace Modle.Migrations
{
    public partial class Exchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "Company",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptAutomaticCurrencyExchange",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IsAcceptAutomaticCurrencyExchange",
                table: "Company");
        }
    }
}
