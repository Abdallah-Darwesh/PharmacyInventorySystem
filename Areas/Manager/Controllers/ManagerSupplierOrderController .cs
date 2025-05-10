using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyInventorySystem.Services;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ManagerSupplierOrderController : Controller
    {
        
        private readonly ISupplierOrderService _supplierOrderService;

        public ManagerSupplierOrderController(ISupplierOrderService supplierOrderService)
        {
            _supplierOrderService = supplierOrderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _supplierOrderService.GetAllOrdersForManagerAsync();
            return View(orders);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _supplierOrderService.GetOrderDetailsByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

    }
}
