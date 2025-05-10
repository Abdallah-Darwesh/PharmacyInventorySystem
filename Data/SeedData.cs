using Microsoft.AspNetCore.Identity;
using PharmacyInventorySystem.Models; // تأكد من إضافة namespace الخاص بـ ApplicationUser

namespace PharmacyInventorySystem.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // الحصول على Manager المناسب للأدوار والمستخدمين
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Manager", "Pharmacist" };

                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // إنشاء مستخدم Manager تجريبي (اختياري)
                var managerEmail = "manager@pharmacy.com";
                var managerUser = await userManager.FindByEmailAsync(managerEmail);
                if (managerUser == null)
                {
                    managerUser = new ApplicationUser
                    {
                        UserName = managerEmail,
                        Email = managerEmail,
                        EmailConfirmed = true,
                        UserType = "Manager",
                        // عند إنشاء الحساب الإداري يمكن تعيين الموافقة تلقائيًا
                        IsApproved = true
                    };
                    var result = await userManager.CreateAsync(managerUser, "123456!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(managerUser, "Manager");
                    }
                }

                // إنشاء مستخدم Pharmacist تجريبي (اختياري)
                var pharmacistEmail = "p@p.com";
                var pharmacistUser = await userManager.FindByEmailAsync(pharmacistEmail);
                if (pharmacistUser == null)
                {
                    pharmacistUser = new ApplicationUser
                    {
                        UserName = pharmacistEmail,
                        Email = pharmacistEmail,
                        EmailConfirmed = true,
                        UserType = "Pharmacist",
                        IsApproved = true
                    };
                    var result = await userManager.CreateAsync(pharmacistUser, "123456!@aA");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(pharmacistUser, "Pharmacist");
                    }
                }
            }
        }
    }
}
