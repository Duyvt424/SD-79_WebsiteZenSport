using AppData.Models;

namespace AppView.Models
{
    public class VoucherViewModel
    {
		public Guid VoucherID { get; set; }
		public string? VoucherCode { get; set; }
		public decimal VoucherValue { get; set; }
		public int MaxUsage { get; set; }
		public int RemainingUsage { get; set; }
		public DateTime ExpirationDate { get; set; }
		public int Status { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
