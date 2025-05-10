using Microsoft.AspNetCore.Mvc;
using PharmacyInventorySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    public class SupplierContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);
        }
    }
}
