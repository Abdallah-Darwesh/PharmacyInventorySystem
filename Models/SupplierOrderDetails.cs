using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyInventorySystem.Models
{
    // Represents the detail of each product in a supplier order.
    public class SupplierOrderDetail
    {
        [Key]
        public int SupplierOrderDetailID { get; set; }

        public int SupplierOrderID { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SUnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SPPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BUnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BPPrice { get; set; }

        [ForeignKey(nameof(SupplierOrderID))]
        public virtual SupplierOrder SupplierOrder { get; set; }

        [ForeignKey(nameof(ProductID))]
        public virtual Product Product { get; set; }
    }

}
