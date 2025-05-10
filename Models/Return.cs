using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.Models
{
    // يمثل سجل المرتجع العام سواء كان من فاتورة البيع أو طلب المورد
    public class Return
    {
        [Key]
        public int ReturnID { get; set; }

        public DateTime ReturnDate { get; set; }

        [Required]
        public string ReturnType { get; set; } // مثال: "SalesReturn" أو "SupplierReturn"

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReturnCost { get; set; }


        // يمكن ربط المرتجع إما بفاتورة بيع أو بطلب مورد (اختياري)
        public int? SaleID { get; set; }
        [ForeignKey("SaleID")]
        public virtual Sale Sale { get; set; }

        public int? SupplierOrderID { get; set; }
        [ForeignKey("SupplierOrderID")]
        public virtual SupplierOrder SupplierOrder { get; set; }

        public virtual ICollection<ReturnDetail> ReturnDetails { get; set; }
    }

}
