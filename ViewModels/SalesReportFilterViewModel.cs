using PharmacyInventorySystem.Models;

namespace PharmacyInventorySystem.ViewModels
{
    public class SalesReportFilterViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string PharmacistId { get; set; }
        public List<ApplicationUser>? Pharmacists { get; set; }
        public List<Sale>? Sales { get; set; }
        public List<string> SaleDates { get; set; } = new();
        public List<decimal> TotalPrices { get; set; } = new();
        public List<string> PharmacistNames { get; set; } = new();
        public List<decimal> SalesByPharmacist { get; set; } = new();
        public List<string> TopProductNames { get; set; } = new();
        public List<int> TopProductQuantities { get; set; } = new();



    }
}
