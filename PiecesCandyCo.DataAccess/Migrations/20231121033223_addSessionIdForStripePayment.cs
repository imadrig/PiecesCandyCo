using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiecesCandyCo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addSessionIdForStripePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "CustomerOrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "CustomerOrderDetails");
        }
    }
}
