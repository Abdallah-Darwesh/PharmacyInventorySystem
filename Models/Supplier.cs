using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyInventorySystem.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        // Initializes the collection to prevent null references.
        public virtual ICollection<SupplierOrder> SupplierOrders { get; set; }

        public Supplier()
        {
            SupplierOrders = new List<SupplierOrder>();
        }
    }
}
