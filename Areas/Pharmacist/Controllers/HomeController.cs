using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
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
            int lowStockThreshold = 10;
            int expiryThresholdDays = 30;
            DateTime today = DateTime.Today;
            DateTime nearExpiry = today.AddDays(expiryThresholdDays);

            var alerts = await _context.Products
                .Where(p => p.Quantity < lowStockThreshold || p.ExDate <= nearExpiry)
                .ToListAsync();

            ViewBag.HasAlerts = alerts.Any();

            // Fetch current user's full name
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user?.FullName ?? user?.UserName;

            return View();
        }

        public async Task<IActionResult> Report()
        {
            int lowStockThreshold = 10;
            int expiryThresholdDays = 30;
            DateTime today = DateTime.Today;

            var products = await _context.Products
                .Where(p =>
                    p.Quantity < lowStockThreshold ||
                    EF.Functions.DateDiffDay(today, p.ExDate) <= expiryThresholdDays)
                .ToListAsync();

            return View(products);
        }


    }
}
