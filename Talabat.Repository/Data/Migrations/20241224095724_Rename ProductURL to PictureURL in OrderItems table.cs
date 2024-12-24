using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.Migrations
{
    public partial class RenameProductURLtoPictureURLinOrderItemstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_ProductUrl",
                table: "OrderItems",
                newName: "Product_PictureUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_PictureUrl",
                table: "OrderItems",
                newName: "Product_ProductUrl");
        }
    }
}
