using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitSupplier2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "signOrderDate",
                table: "SupplierOrders",
                newName: "SignOrderDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignOrderDate",
                table: "SupplierOrders",
                newName: "signOrderDate");
        }
    }
}
