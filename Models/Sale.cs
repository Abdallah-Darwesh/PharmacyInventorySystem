using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyInventorySystem.Models
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        public DateTime SaleDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // ✅ ربط الـ Sale بالصيدلي اللي عمله
        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<SalesDetail> SalesDetails { get; set; }
        public virtual ICollection<Return> Returns { get; set; }
    }
}
