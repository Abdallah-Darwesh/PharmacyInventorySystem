using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.ViewModels
{
    public class AddProductsToOrderViewModel
    {
        public int SupplierOrderID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal SUnitPrice { get; set; }

        [Required]
        public decimal SPPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime ExDate { get; set; } = DateTime.Now.AddYears(1);

        public string PatchNom { get; set; } = "AUTO";

        public int CategoryID { get; set; }
    }

}
