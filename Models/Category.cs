using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PharmacyInventorySystem.Models
{
    // يمثل التصنيفات الخاصة بالمنتجات
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [ValidateNever]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
