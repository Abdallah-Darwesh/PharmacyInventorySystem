namespace PharmacyInventorySystem.ViewModels
{
    public class ManagerSupplierOrderDetailsViewModel
    {
        public int SupplierOrderID { get; set; }
        public string SupplierName { get; set; }
        public string PharmacistName { get; set; }
        public DateTime SignOrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderProductViewModel> Products { get; set; } = new();
    }

}
