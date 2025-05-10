using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyInventorySystem.Models
{
    // يمثل المنتجات
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        // الأسعار تُخزن باستخدام Decimal لمنع مشاكل الدقة
        [Column(TypeName = "decimal(18,2)")]
        public decimal SUnitPrice { get; set; }  // سعر البيع لوحدة (مثال: بيع حبة)

        [Column(TypeName = "decimal(18,2)")]
        public decimal SPPrice { get; set; }     // سعر البيع للعبوة أو حسب الحاجة

        [Column(TypeName = "decimal(18,2)")]
        public decimal BUnitPrice { get; set; }    // سعر الشراء بالوحدة

        [Column(TypeName = "decimal(18,2)")]
        public decimal BPPrice { get; set; }      // سعر الشراء للعبوة أو حسب الحاجة

        public int Quantity { get; set; }
        public DateTime ExDate { get; set; }
        public string PatchNom { get; set; }

        // العلاقة مع التصنيف
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        // علاقات مع تفاصيل المبيعات والطلبات والمرتجعات
        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
        public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
        public virtual ICollection<ReturnDetail> ReturnDetails { get; set; }
    }
}
