namespace PharmacyInventorySystem.ViewModels
{
    public class SaleItemViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string SaleType { get; set; } // "Unit" or "Package"
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }


}
