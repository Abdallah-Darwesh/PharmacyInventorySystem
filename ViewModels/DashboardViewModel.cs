// PharmacyInventorySystem/ViewModels/DashboardViewModel.cs
namespace PharmacyInventorySystem.ViewModels
{
    public class DashboardViewModel
    {
        public int EmployeeCount { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalReturns { get; set; }
        public int InventoryCount { get; set; }
        public int TotalOrders { get; set; }      // بدل PendingOrders
        public int LowStockAlerts { get; set; }

        public bool HasAlerts => LowStockAlerts > 0;
    }
}
