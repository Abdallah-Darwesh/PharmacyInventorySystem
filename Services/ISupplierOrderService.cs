using PharmacyInventorySystem.ViewModels;

namespace PharmacyInventorySystem.Services
{
    public interface ISupplierOrderService
    {
        Task<List<ManagerSupplierOrderViewModel
>> GetAllOrdersForManagerAsync();

        Task<ManagerSupplierOrderDetailsViewModel?> GetOrderDetailsByIdAsync(int orderId);

    }
}