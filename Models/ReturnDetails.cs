using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.Models
{
    // يمثل تفصيل كل بند في المرتجع (المنتجات المُردة وتفاصيلها)
    public class ReturnDetail
    {
        [Key]
        public int ReturnDetailID { get; set; }

        public int ReturnID { get; set; }
        public int ProductID { get; set; }

        public int ReturnQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReturnUnitCost { get; set; }

        [ForeignKey("ReturnID")]
        public virtual Return Return { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
