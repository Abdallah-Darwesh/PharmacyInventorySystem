namespace PharmacyInventorySystem.ViewModels
{
    public class ReturnDetailItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total => Quantity * UnitCost;
    }
}
