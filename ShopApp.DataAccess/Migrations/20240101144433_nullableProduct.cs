using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApp.DataAccess.Migrations
{
    public partial class nullableProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_ProductCategory_ProductCategoryCategoryId_ProductCategoryProductId",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_ProductCategoryCategoryId_ProductCategoryProductId",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "ProductCategoryCategoryId",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "ProductCategoryProductId",
                table: "ProductCategory");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryCategoryId",
                table: "ProductCategory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryProductId",
                table: "ProductCategory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductCategoryCategoryId_ProductCategoryProductId",
                table: "ProductCategory",
                columns: new[] { "ProductCategoryCategoryId", "ProductCategoryProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_ProductCategory_ProductCategoryCategoryId_ProductCategoryProductId",
                table: "ProductCategory",
                columns: new[] { "ProductCategoryCategoryId", "ProductCategoryProductId" },
                principalTable: "ProductCategory",
                principalColumns: new[] { "CategoryId", "ProductId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
