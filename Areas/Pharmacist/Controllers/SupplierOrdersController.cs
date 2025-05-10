using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using PharmacyInventorySystem.ViewModels;
using Rotativa.AspNetCore;



namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class SupplierOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        
        private readonly IConverter _converter;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupplierOrdersController(ApplicationDbContext context, IConverter converter, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            
            _converter = converter;
            _userManager = userManager;

        }


        // GET: Pharmacist/SupplierOrders/Create
        public IActionResult Create()
        {
            ViewBag.SupplierList = new SelectList(_context.Suppliers, "SupplierID", "SupplierName");
            return View();
        }


        // POST: Pharmacist/SupplierOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierOrderCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var supplierOrder = new SupplierOrder
                {
                    SupplierID = vm.SupplierID,
                    TotalAmount = vm.TotalAmount,
                    SignOrderDate = DateTime.Now,
                    CreatedByUserId = _userManager.GetUserId(User) // ✅ السطر المهم
                };

                _context.Add(supplierOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddProduct", new { id = supplierOrder.SupplierOrderID });
            }


            ViewBag.SupplierList = new SelectList(_context.Suppliers, "SupplierID", "SupplierName", vm.SupplierID);
            return View(vm);
        }
        // GET: Pharmacist/SupplierOrders/AddProduct/{supplierOrderId}
        public async Task<IActionResult> AddProduct(int id)
        {
            var viewModel = new AddProductToSupplierOrderViewModel
            {
                SupplierOrderID = id,
                Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "CategoryName")
            };

            var existingProducts = await _context.SupplierOrderDetails
                .Include(s => s.Product)
                .Where(s => s.SupplierOrderID == id)
                .ToListAsync();

            ViewBag.ExistingProducts = existingProducts;

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductToSupplierOrderViewModel vm, string actionType)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = _context.Categories
                    .Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.CategoryName })
                    .ToList();
                return View(vm);
            }

            // إنشاء المنتج
            var product = new Product
            {
                ProductName = vm.ProductName,
                SUnitPrice = vm.SUnitPrice,
                SPPrice = vm.SPPrice,
                BUnitPrice = vm.BUnitPrice,
                BPPrice = vm.BPPrice,
                Quantity = vm.Quantity,
                ExDate = vm.ExDate,
                PatchNom = vm.PatchNom,
                CategoryID = vm.CategoryID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // إنشاء تفاصيل الطلب
            var orderDetail = new SupplierOrderDetail
            {
                SupplierOrderID = vm.SupplierOrderID,
                ProductID = product.ProductID,
                Quantity = vm.Quantity,
                SUnitPrice = vm.SUnitPrice,
                SPPrice = vm.SPPrice
            };

            _context.SupplierOrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            if (actionType == "add")
            {
                // إعادة فتح نفس الصفحة لإضافة منتج آخر
                return RedirectToAction("AddProduct", new { id = vm.SupplierOrderID });
            }
            else if (actionType == "finish")
            {
                // الذهاب لصفحة الملخص
                return RedirectToAction("Summary", new { id = vm.SupplierOrderID });
            }

            // fallback
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Summary(int id)
        {
            var supplierOrder = await _context.SupplierOrders
                .Include(o => o.Supplier)
                .Include(o => o.SupplierOrderDetails)
                    .ThenInclude(d => d.Product)
                    .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.SupplierOrderID == id);

            if (supplierOrder == null)
            {
                return NotFound();
            }

            return View(supplierOrder);
        }

        public async Task<IActionResult> PrintPdf(int id)
        {
            var supplierOrder = await _context.SupplierOrders
                .Include(o => o.Supplier)
                .Include(o => o.CreatedByUser)
                .Include(o => o.SupplierOrderDetails)
                    .ThenInclude(d => d.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.SupplierOrderID == id);

            if (supplierOrder == null)
            {
                return NotFound();
            }

            var vm = new PharmacyInventorySystem.ViewModels.PharmacistSupplierOrderDetailsViewModel
            {
                SupplierOrderID = supplierOrder.SupplierOrderID,
                SupplierName = supplierOrder.Supplier?.SupplierName,
                PharmacistName = supplierOrder.CreatedByUser?.FullName ?? supplierOrder.CreatedByUser?.UserName,
                SignOrderDate = supplierOrder.SignOrderDate,
                TotalAmount = supplierOrder.TotalAmount,
                Products = supplierOrder.SupplierOrderDetails.Select(d => new PharmacyInventorySystem.ViewModels.PharmacistOrderProductViewModel
                {
                    ProductName = d.Product.ProductName,
                    Quantity = d.Quantity,
                    SUnitPrice = d.SUnitPrice,
                    SPPrice = d.SPPrice,
                    BUnitPrice = d.BUnitPrice,
                    BPPrice = d.BPPrice,
                    PatchNom = d.Product.PatchNom,
                    ExDate = d.Product.ExDate,
                    CategoryName = d.Product.Category?.CategoryName
                }).ToList()
            };

            return new Rotativa.AspNetCore.ViewAsPdf("PrintSupplierOrder", vm)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                FileName = $"SupplierOrder_{id}.pdf"
            };
        }



    }
}

