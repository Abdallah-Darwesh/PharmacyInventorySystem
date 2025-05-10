using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyInventorySystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPharmacyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SPPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BPPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ExDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatchNom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierOrders",
                columns: table => new
                {
                    SupplierOrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    signOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOrders", x => x.SupplierOrderID);
                    table.ForeignKey(
                        name: "FK_SupplierOrders_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesDetails",
                columns: table => new
                {
                    SalesDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDetails", x => x.SalesDetailID);
                    table.ForeignKey(
                        name: "FK_SalesDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesDetails_Sales_SaleID",
                        column: x => x.SaleID,
                        principalTable: "Sales",
                        principalColumn: "SaleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    ReturnID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalReturnCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaleID = table.Column<int>(type: "int", nullable: true),
                    SupplierOrderID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.ReturnID);
                    table.ForeignKey(
                        name: "FK_Returns_Sales_SaleID",
                        column: x => x.SaleID,
                        principalTable: "Sales",
                        principalColumn: "SaleID");
                    table.ForeignKey(
                        name: "FK_Returns_SupplierOrders_SupplierOrderID",
                        column: x => x.SupplierOrderID,
                        principalTable: "SupplierOrders",
                        principalColumn: "SupplierOrderID");
                });

            migrationBuilder.CreateTable(
                name: "SupplierOrderDetails",
                columns: table => new
                {
                    SupplierOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierOrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SPPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOrderDetails", x => x.SupplierOrderDetailID);
                    table.ForeignKey(
                        name: "FK_SupplierOrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierOrderDetails_SupplierOrders_SupplierOrderID",
                        column: x => x.SupplierOrderID,
                        principalTable: "SupplierOrders",
                        principalColumn: "SupplierOrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnDetails",
                columns: table => new
                {
                    ReturnDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ReturnQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnUnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnDetails", x => x.ReturnDetailID);
                    table.ForeignKey(
                        name: "FK_ReturnDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnDetails_Returns_ReturnID",
                        column: x => x.ReturnID,
                        principalTable: "Returns",
                        principalColumn: "ReturnID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnDetails_ProductID",
                table: "ReturnDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnDetails_ReturnID",
                table: "ReturnDetails",
                column: "ReturnID");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_SaleID",
                table: "Returns",
                column: "SaleID");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_SupplierOrderID",
                table: "Returns",
                column: "SupplierOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_ProductID",
                table: "SalesDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_SaleID",
                table: "SalesDetails",
                column: "SaleID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrderDetails_ProductID",
                table: "SupplierOrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrderDetails_SupplierOrderID",
                table: "SupplierOrderDetails",
                column: "SupplierOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_SupplierID",
                table: "SupplierOrders",
                column: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnDetails");

            migrationBuilder.DropTable(
                name: "SalesDetails");

            migrationBuilder.DropTable(
                name: "SupplierOrderDetails");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "SupplierOrders");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
