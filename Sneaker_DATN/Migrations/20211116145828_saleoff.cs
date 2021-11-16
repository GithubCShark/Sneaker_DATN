using Microsoft.EntityFrameworkCore.Migrations;

namespace Sneaker_DATN.Migrations
{
    public partial class saleoff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Sale",
                table: "Products",
                type: "money",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "sale");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Sale",
                table: "Products",
                type: "sale",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
