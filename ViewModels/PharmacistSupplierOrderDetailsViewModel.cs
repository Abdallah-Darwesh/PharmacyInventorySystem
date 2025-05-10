namespace PharmacyInventorySystem.ViewModels
{
    public class PharmacistSupplierOrderDetailsViewModel
    {
        public int SupplierOrderID { get; set; }
        public string SupplierName { get; set; }
        public string PharmacistName { get; set; }
        public DateTime SignOrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PharmacistOrderProductViewModel> Products { get; set; } = new();
    }

    public class PharmacistOrderProductViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal SUnitPrice { get; set; }
        public decimal SPPrice { get; set; }
        public decimal BUnitPrice { get; set; }
        public decimal BPPrice { get; set; }
        public string PatchNom { get; set; }
        public DateTime ExDate { get; set; }
        public string CategoryName { get; set; }
    }
} 