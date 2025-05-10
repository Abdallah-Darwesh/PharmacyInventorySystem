using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using PharmacyInventorySystem.ViewModels;

namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class ReturnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReturnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // صفحة اختيار نوع المرتجع
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectReference(string ReturnType, int ReferenceID)
        {
            if (ReturnType == "SalesReturn")
            {
                var sale = await _context.Sales
                    .Include(s => s.SalesDetails)
                    .ThenInclude(d => d.Product)
                    .FirstOrDefaultAsync(s => s.SaleID == ReferenceID);

                if (sale == null) return NotFound();

                var vm = new CreateReturnViewModel
                {
                    ReturnType = ReturnType,
                    SaleID = sale.SaleID,
                    Sale = sale,
                    ReturnItems = sale.SalesDetails.Select(sd => new ReturnItemViewModel
                    {
                        ProductID = sd.ProductID,
                        ProductName = sd.Product.ProductName,
                        MaxQuantity = sd.Quantity,
                        UnitPrice = sd.UnitPrice
                    }).ToList()
                };

                return View("CreateSalesReturn", vm);
            }
            else if (ReturnType == "SupplierReturn")
            {
                var order = await _context.SupplierOrders
                    .Include(o => o.SupplierOrderDetails)
                    .ThenInclude(d => d.Product)
                    .FirstOrDefaultAsync(o => o.SupplierOrderID == ReferenceID);

                if (order == null || order.SupplierOrderDetails == null || !order.SupplierOrderDetails.Any())
                {
                    ModelState.AddModelError("", "Supplier order not found or has no items.");
                    return View("Create");
                }

                var vm = new CreateReturnViewModel
                {
                    ReturnType = ReturnType,
                    SupplierOrderID = order.SupplierOrderID,
                    SupplierOrder = order,
                    ReturnItems = order.SupplierOrderDetails.Select(od => new ReturnItemViewModel
                    {
                        ProductID = od.ProductID,
                        ProductName = od.Product.ProductName,
                        MaxQuantity = od.Quantity,
                        UnitPrice = od.Product.BPPrice
                    }).ToList()
                };

                return View("CreateSupplierReturn", vm);
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSalesReturn(CreateReturnViewModel model)
        {
            if (model.SaleID == null || model.ReturnItems == null || !model.ReturnItems.Any())
                return BadRequest();

            var returnEntry = new Return
            {
                ReturnDate = DateTime.Now,
                ReturnType = "SalesReturn",
                SaleID = model.SaleID,
                TotalReturnCost = 0,
                ReturnDetails = new List<ReturnDetail>()
            };

            foreach (var item in model.ReturnItems)
            {
                if (item.ReturnQuantity <= 0 || item.ReturnQuantity > item.MaxQuantity) continue;

                var product = await _context.Products.FindAsync(item.ProductID);
                if (product == null) continue;

                var returnDetail = new ReturnDetail
                {
                    ProductID = item.ProductID,
                    ReturnQuantity = item.ReturnQuantity,
                    ReturnUnitCost = item.UnitPrice,
                    Return = returnEntry
                };

                returnEntry.ReturnDetails.Add(returnDetail);
                product.Quantity += item.ReturnQuantity;
                returnEntry.TotalReturnCost += item.ReturnQuantity * item.UnitPrice;
            }

            _context.Returns.Add(returnEntry);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = returnEntry.ReturnID });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSupplierReturn(CreateReturnViewModel model)
        {
            if (model.SupplierOrderID == null || model.ReturnItems == null || !model.ReturnItems.Any())
                return BadRequest();

            var returnEntry = new Return
            {
                ReturnDate = DateTime.Now,
                ReturnType = "SupplierReturn",
                SupplierOrderID = model.SupplierOrderID,
                TotalReturnCost = 0,
                ReturnDetails = new List<ReturnDetail>()
            };

            foreach (var item in model.ReturnItems)
            {
                if (item.ReturnQuantity <= 0 || item.ReturnQuantity > item.MaxQuantity) continue;

                var product = await _context.Products.FindAsync(item.ProductID);
                if (product == null) continue;

                var returnDetail = new ReturnDetail
                {
                    ProductID = item.ProductID,
                    ReturnQuantity = item.ReturnQuantity,
                    ReturnUnitCost = item.UnitPrice,
                    Return = returnEntry
                };

                returnEntry.ReturnDetails.Add(returnDetail);
                product.Quantity -= item.ReturnQuantity;
                returnEntry.TotalReturnCost += item.ReturnQuantity * item.UnitPrice;
            }

            _context.Returns.Add(returnEntry);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = returnEntry.ReturnID });
        }


        public async Task<IActionResult> Details(int id)
        {
            var returnEntry = await _context.Returns
                .Include(r => r.ReturnDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(r => r.ReturnID == id);

            if (returnEntry == null) return NotFound();

            return View(returnEntry);
        }

    }
}
