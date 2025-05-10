// Areas/Manager/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PharmacyInventorySystem.Models;
using System.Threading.Tasks;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var vm = new DashboardViewModel
            {
                EmployeeCount = _context.Users.Count(),
                TotalSales = _context.Sales.Sum(s => s.TotalPrice),
                TotalReturns = _context.Returns.Count(),
                InventoryCount = _context.Products.Count(),

                // إجمالي أوامر الموردين المسجّلة
                TotalOrders = _context.SupplierOrders.Count(),

                // تنبيهات المخزون: الكمية صفر
                LowStockAlerts = _context.Products.Count(p => p.Quantity == 0)
            };

            // Fetch current user's full name
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user?.FullName ?? user?.UserName;

            return View(vm);
        }
    }
}
