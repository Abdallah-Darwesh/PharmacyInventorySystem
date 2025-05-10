namespace PharmacyInventorySystem.ViewModels
{
    public class ReturnReportViewModel
    {
        public int ReturnID { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReturnType { get; set; }
        public decimal TotalReturnCost { get; set; }

        public List<ReturnDetailItem> Details { get; set; }
    }

}
