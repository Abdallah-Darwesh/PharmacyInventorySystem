using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.ViewModels
{
    public class ReturnItemViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int MaxQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReturnQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
