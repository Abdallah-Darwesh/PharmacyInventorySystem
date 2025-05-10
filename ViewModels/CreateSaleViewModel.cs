 namespace PharmacyInventorySystem.ViewModels
{
    public class CreateSaleViewModel
    {
        public List<SaleItemViewModel> SaleItems { get; set; } = new();
        public decimal GrandTotal => SaleItems.Sum(x => x.SubTotal);
    }

}
