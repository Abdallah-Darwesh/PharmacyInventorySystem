namespace PharmacyInventorySystem.ViewModels
{
    public class ManagerSupplierOrderViewModel
    {
        public int SupplierOrderID { get; set; }
        public string SupplierName { get; set; } = null!;
        public DateTime SignOrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PharmacistName { get; set; } = null!;

        public int ProductsCount { get; set; }
    }
}
