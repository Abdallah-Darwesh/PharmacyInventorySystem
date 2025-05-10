using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.SupplierOrders
                .Include(o => o.Supplier)
                .Include(o => o.SupplierOrderDetails)
                .Include(o => o.CreatedByUser) // هنضيفها تحت في الكلاس لو مش موجودة
                .ToListAsync();

            return View(orders);
        }
    }
}
