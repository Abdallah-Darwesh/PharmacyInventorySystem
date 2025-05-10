using Microsoft.AspNetCore.Identity;

namespace PharmacyInventorySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // خاصية لتحديد نوع المستخدم: Manager أو Pharmacist
        public string UserType { get; set; }
        public string? FullName { get; set; }

        // خاصية للموافقة (إذا كنت تريد أن يتم التحكم في الدخول بناءً عليها)
        public bool IsApproved { get; set; } = false;
    }
}
