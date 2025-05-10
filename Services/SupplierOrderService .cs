using System;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace PharmacyInventorySystem.Services
{
    public class SupplierOrderService : ISupplierOrderService
    {
        private readonly ApplicationDbContext _context;

        public SupplierOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ManagerSupplierOrderViewModel
>> GetAllOrdersForManagerAsync()
        {
            return await _context.SupplierOrders
                .Include(o => o.Supplier)
                .Include(o => o.CreatedByUser)
                .Include(o => o.SupplierOrderDetails)
                .Select(o => new ManagerSupplierOrderViewModel
                {
                    SupplierOrderID = o.SupplierOrderID,
                    SupplierName = o.Supplier.SupplierName,
                    SignOrderDate = o.SignOrderDate,
                    TotalAmount = o.TotalAmount,
                    PharmacistName = o.CreatedByUser.FullName ?? o.CreatedByUser.UserName,
                    ProductsCount = o.SupplierOrderDetails.Count
                })
                .ToListAsync();
        }



        public async Task<ManagerSupplierOrderDetailsViewModel?> GetOrderDetailsByIdAsync(int orderId)
        {
            return await _context.SupplierOrders
                .Where(o => o.SupplierOrderID == orderId)
                .Include(o => o.Supplier)
                .Include(o => o.CreatedByUser)
                .Include(o => o.SupplierOrderDetails)
                    .ThenInclude(d => d.Product)
                .Select(o => new ManagerSupplierOrderDetailsViewModel
                {
                    SupplierOrderID = o.SupplierOrderID,
                    SupplierName = o.Supplier.SupplierName,
                    PharmacistName = o.CreatedByUser.FullName ?? o.CreatedByUser.UserName,
                    SignOrderDate = o.SignOrderDate,
                    TotalAmount = o.TotalAmount,
                    Products = o.SupplierOrderDetails.Select(d => new OrderProductViewModel
                    {
                        ProductName = d.Product.ProductName,
                        Quantity = d.Quantity,
                        BUnitPrice = d.BUnitPrice,
                        BPPrice = d.BPPrice,
                        SUnitPrice = d.SUnitPrice,
                        SPPrice = d.SPPrice,
                        ExDate = d.Product.ExDate,
                        PatchNom = d.Product.PatchNom
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }



    }
}