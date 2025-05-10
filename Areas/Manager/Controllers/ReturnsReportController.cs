using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.ViewModels;
using Rotativa.AspNetCore;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ReturnsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReturnsReportController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string returnType = null)
        {
            var query = _context.Returns
                .Include(r => r.ReturnDetails)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(r => r.ReturnDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.ReturnDate <= toDate.Value);

            if (!string.IsNullOrEmpty(returnType))
                query = query.Where(r => r.ReturnType == returnType);

            var data = await query
                .OrderByDescending(r => r.ReturnDate)
                .Select(r => new ReturnReportViewModel
                {
                    ReturnID = r.ReturnID,
                    ReturnDate = r.ReturnDate,
                    ReturnType = r.ReturnType,
                    TotalReturnCost = r.TotalReturnCost,
                    Details = r.ReturnDetails.Select(d => new ReturnDetailItem
                    {
                        ProductName = d.Product != null ? d.Product.ProductName : "Unknown Product",
                        Quantity = d.ReturnQuantity,
                        UnitCost = d.ReturnUnitCost
                    }).ToList()
                })
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Print(DateTime? fromDate, DateTime? toDate, string returnType = null)
        {
            var query = _context.Returns
                .Include(r => r.ReturnDetails)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(r => r.ReturnDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.ReturnDate <= toDate.Value);

            if (!string.IsNullOrEmpty(returnType))
                query = query.Where(r => r.ReturnType == returnType);

            var data = await query
                .OrderByDescending(r => r.ReturnDate)
                .Select(r => new ReturnReportViewModel
                {
                    ReturnID = r.ReturnID,
                    ReturnDate = r.ReturnDate,
                    ReturnType = r.ReturnType,
                    TotalReturnCost = r.TotalReturnCost,
                    Details = r.ReturnDetails.Select(d => new ReturnDetailItem
                    {
                        ProductName = d.Product != null ? d.Product.ProductName : "Unknown Product",
                        Quantity = d.ReturnQuantity,
                        UnitCost = d.ReturnUnitCost
                    }).ToList()
                })
                .ToListAsync();

            return new ViewAsPdf("Print", data)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                FileName = "ReturnsReport.pdf"
            };
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnRecord = await _context.Returns
                .Include(r => r.ReturnDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(r => r.ReturnID == id);

            if (returnRecord == null)
                return NotFound();

            var viewModel = new ReturnReportViewModel
            {
                ReturnID = returnRecord.ReturnID,
                ReturnDate = returnRecord.ReturnDate,
                ReturnType = returnRecord.ReturnType,
                TotalReturnCost = returnRecord.TotalReturnCost,
                Details = returnRecord.ReturnDetails.Select(d => new ReturnDetailItem
                {
                    ProductName = d.Product != null ? d.Product.ProductName : "منتج غير موجود",
                    Quantity = d.ReturnQuantity,
                    UnitCost = d.ReturnUnitCost
                }).ToList()
            };

            return View(viewModel);
        }




    }
}
