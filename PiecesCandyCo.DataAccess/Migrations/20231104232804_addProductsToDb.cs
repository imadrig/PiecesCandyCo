using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PiecesCandyCo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addProductsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price10 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price", "Price10" },
                values: new object[,]
                {
                    { 301, "Perfect for any occasion, these gummy bears come infused with flavors of champagne.", "Champagne Gummy Bears (12 PK)", 10.949999999999999, 8.9499999999999993 },
                    { 302, "Perfect for any occasion, these gummy bears come infused with flavors of rosé wine.", "Rosé Gummy Bears (12 PK)", 10.949999999999999, 8.9499999999999993 },
                    { 303, "Perfect for any occasion, this chocolate bar is topped with peppermint and marshmallows.", "Winter Wonderland White Chocolate Bar", 14.949999999999999, 12.949999999999999 },
                    { 304, "Perfect for any occasion, this chocolate bar is filled with liquid cappuccino.", "Cappuccino Chocolate Bar", 14.949999999999999, 12.949999999999999 },
                    { 305, "Perfect for any occasion, these candies come infused with flavors of whiskey.", "Whiskey Sour Hard Candies (12 PK)", 10.949999999999999, 8.9499999999999993 },
                    { 306, "Perfect for any occasion, these candies come in pineapple flavor.", "Pineapple Candy (12 PK)", 10.949999999999999, 8.9499999999999993 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
