using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPharmacistToSale_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ApplicationUserId",
                table: "Sales",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_AspNetUsers_ApplicationUserId",
                table: "Sales",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_AspNetUsers_ApplicationUserId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ApplicationUserId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Sales");
        }
    }
}
