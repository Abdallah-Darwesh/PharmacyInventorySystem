using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyInventorySystem.Models
{
    public class SupplierOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierOrderID { get; set; }

        public DateTime SignOrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string? CreatedByUserId { get; set; } // ✅ خليها nullable (بدون [Required])

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual ApplicationUser CreatedByUser { get; set; }


        // تغيير النوع إلى int (غير nullable) مع تحقق أن القيمة تكون أكبر من 0
        [Required(ErrorMessage = "The Supplier field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Supplier field is required.")]
        public int SupplierID { get; set; }

        [ForeignKey(nameof(SupplierID))]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
        public virtual ICollection<Return> Returns { get; set; }

        public SupplierOrder()
        {
            SupplierOrderDetails = new List<SupplierOrderDetail>();
            Returns = new List<Return>();
        }
    }
}
