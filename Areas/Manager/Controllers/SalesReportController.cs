using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using PharmacyInventorySystem.ViewModels;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class SalesReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesReportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string pharmacistId)
        {
            var pharmacists = await _userManager.GetUsersInRoleAsync("Pharmacist");

            var query = _context.Sales
                .Include(s => s.SalesDetails)
                .Include(s => s.ApplicationUser)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(s => s.SaleDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(s => s.SaleDate <= toDate.Value);

            if (!string.IsNullOrEmpty(pharmacistId))
                query = query.Where(s => s.ApplicationUserId == pharmacistId);

            var salesList = await query
                .Where(s => s.ApplicationUser != null) // تأكيد أن الـ ApplicationUser موجود
                .OrderBy(s => s.SaleDate)
                .ToListAsync();

            var salesByPharmacist = salesList
                .GroupBy(s => s.ApplicationUser.FullName)
                .Select(g => new
                {
                    Pharmacist = g.Key,
                    TotalSales = g.Sum(s => s.TotalPrice)
                })
                .ToList();
            var sales = await _context.Sales
                .Include(s => s.SalesDetails)
                    .ThenInclude(sd => sd.Product)
                .ToListAsync();

            var topProducts = sales
                .SelectMany(s => s.SalesDetails)  // تجمع كل تفاصيل البيع من كل عملية بيع
                .Where(sd => sd.Product != null)
                .GroupBy(sd => sd.Product.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToList();




            var model = new SalesReportFilterViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                PharmacistId = pharmacistId,
                Pharmacists = pharmacists.ToList(),
                Sales = salesList,
                SaleDates = salesList.Select(s => s.SaleDate.ToString("yyyy-MM-dd")).ToList(),
                TotalPrices = salesList.Select(s => s.TotalPrice).ToList(),
                PharmacistNames = salesByPharmacist.Select(p => p.Pharmacist).ToList(),
                SalesByPharmacist = salesByPharmacist.Select(p => p.TotalSales).ToList(),


                TopProductNames = topProducts.Select(p => p.ProductName).ToList(),
                TopProductQuantities = topProducts.Select(p => p.TotalQuantity).ToList()

            };


            return View(model);
        }
    }
}
