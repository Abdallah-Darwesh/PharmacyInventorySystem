// ViewModels/AddProductToSupplierOrderViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.ViewModels
{
    public class AddProductToSupplierOrderViewModel
    {
        // Supplier Order
        public int SupplierOrderID { get; set; }

        // Product
        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal SUnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal SPPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BUnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BPPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public DateTime ExDate { get; set; }

        public string PatchNom { get; set; }

        [Required]
        public int CategoryID { get; set; }

        // Categories dropdown
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? Categories { get; set; }
    }
}
