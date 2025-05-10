namespace PharmacyInventorySystem.ViewModels
{
    public class OrderProductViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal BUnitPrice { get; set; }
        public decimal BPPrice { get; set; }
        public decimal SUnitPrice { get; set; }
        public decimal SPPrice { get; set; }
        public DateTime ExDate { get; set; }
        public string? PatchNom { get; set; }

    }

}
