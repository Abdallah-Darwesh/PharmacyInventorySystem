using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using PharmacyInventorySystem.ViewModels;
using Rotativa.AspNetCore;

namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Create()
        {
            return View(new CreateSaleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(CreateSaleViewModel model)
        {
            if (!ModelState.IsValid || model.SaleItems == null || !model.SaleItems.Any())
            {
                return View("Create", model);
            }

            var user = await _userManager.GetUserAsync(User);

            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                TotalPrice = model.SaleItems.Sum(x => x.UnitPrice * x.Quantity),
                ApplicationUserId = user.Id, // ✅ ربط الصيدلي بالبيع
                SalesDetails = model.SaleItems.Select(x => new SalesDetail
                {
                    ProductID = x.ProductID,
                    Quantity = x.Quantity,
                    SaleType = x.SaleType,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            // تحديث كمية المنتج
            foreach (var item in sale.SalesDetails)
            {
                var product = await _context.Products.FindAsync(item.ProductID);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }

            _context.Sales.Add(sale); 
            await _context.SaveChangesAsync();

            return RedirectToAction("Invoice", new { id = sale.SaleID });
        }

        public async Task<IActionResult> Invoice(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.SalesDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(s => s.SaleID == id);

            if (sale == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            ViewBag.PharmacistFullName = user.FullName;

            return View(sale);
        }



        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var products = await _context.Products
                .Where(p => p.ProductName.Contains(term))
                .Select(p => new
                {
                    productID = p.ProductID,
                    productName = p.ProductName,
                    sUnitPrice = p.SUnitPrice,
                    sPPrice = p.SPPrice
                })
                .ToListAsync();

            return Json(products);

        }
    }

}
