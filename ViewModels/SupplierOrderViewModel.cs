using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.ViewModels
{
    public class SupplierOrderCreateViewModel
    {
        [Required(ErrorMessage = "The Supplier field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a supplier.")]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }
    }
}
