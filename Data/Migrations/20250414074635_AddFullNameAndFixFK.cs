using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameAndFixFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SupplierOrders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_CreatedByUserId",
                table: "SupplierOrders",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOrders_AspNetUsers_CreatedByUserId",
                table: "SupplierOrders",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOrders_AspNetUsers_CreatedByUserId",
                table: "SupplierOrders");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOrders_CreatedByUserId",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
