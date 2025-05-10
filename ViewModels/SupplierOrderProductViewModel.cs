using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.ViewModels
{
    public class SupplierOrderProductViewModel
    {
        [Required]
        public int SupplierOrderID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal SUnitPrice { get; set; }

        [Required]
        public decimal SPPrice { get; set; }

        [Required]
        public decimal BUnitPrice { get; set; }

        [Required]
        public decimal BPPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime ExDate { get; set; }

        public string PatchNom { get; set; }

        [Required]
        public int CategoryID { get; set; }
    }
}
