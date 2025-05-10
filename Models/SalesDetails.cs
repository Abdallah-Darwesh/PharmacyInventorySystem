using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.Models
{
    // يمثل العناصر الفردية داخل كل فاتورة مبيعات (علاقة many-to-many بين Sales و Products)
    public class SalesDetail
    {
        [Key]
        public int SalesDetailID { get; set; }

        public int SaleID { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("SaleID")]
        public virtual Sale Sale { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        public string SaleType { get; set; } // "Unit" or "Package"

    }

}
