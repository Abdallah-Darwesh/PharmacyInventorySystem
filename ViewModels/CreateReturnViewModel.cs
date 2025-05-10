using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.ViewModels
{
    public class CreateReturnViewModel
    {
        public string ReturnType { get; set; }

        public int? SaleID { get; set; }
        public Sale Sale { get; set; }

        public int? SupplierOrderID { get; set; }
        public SupplierOrder SupplierOrder { get; set; }

        public List<ReturnItemViewModel> ReturnItems { get; set; } = new();
    }
}
