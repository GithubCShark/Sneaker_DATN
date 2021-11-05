using Microsoft.EntityFrameworkCore.Migrations;

namespace Sneaker_DATN.Migrations
{
    public partial class newDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ProductID",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductID",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeID",
                table: "ProductSizes");

            migrationBuilder.RenameColumn(
                name: "SizeID",
                table: "ProductSizes",
                newName: "IdSize");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductSizes",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_SizeID",
                table: "ProductSizes",
                newName: "IX_ProductSizes_IdSize");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductColors",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ID",
                table: "ProductColors",
                column: "ID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ID",
                table: "ProductSizes",
                column: "ID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_IdSize",
                table: "ProductSizes",
                column: "IdSize",
                principalTable: "Sizes",
                principalColumn: "SizeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ID",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ID",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_IdSize",
                table: "ProductSizes");

            migrationBuilder.RenameColumn(
                name: "IdSize",
                table: "ProductSizes",
                newName: "SizeID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ProductSizes",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_IdSize",
                table: "ProductSizes",
                newName: "IX_ProductSizes_SizeID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ProductColors",
                newName: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ProductID",
                table: "ProductColors",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductID",
                table: "ProductSizes",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeID",
                table: "ProductSizes",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "SizeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
