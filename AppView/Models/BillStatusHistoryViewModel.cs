using AppData.Models;

namespace AppView.Models
{
    public class BillStatusHistoryViewModel
    {
        public Guid BillStatusHistoryID { get; set; }
        public int Status { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Note { get; set; }
        public string BillCode { get; set; }
        public string? EmployeeName { get; set; }
    }
}
