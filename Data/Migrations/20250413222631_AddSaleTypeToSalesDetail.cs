using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleTypeToSalesDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaleType",
                table: "SalesDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleType",
                table: "SalesDetails");
        }
    }
}
