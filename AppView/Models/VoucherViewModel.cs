using AppData.Models;

namespace AppView.Models
{
    public class VoucherViewModel
    {
		public Guid VoucherID { get; set; }
		public string? VoucherCode { get; set; }
		public string? Exclusiveright { get; set; }
		public decimal VoucherValue { get; set; }
		public decimal Total { get; set; }
		public int MaxUsage { get; set; }
		public int RemainingUsage { get; set; }
		public DateTime ExpirationDate { get; set; }
		public int Status { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsDel { get; set; }
		public DateTime CreateDate { get; set; }
		public int? Type { get; set; }
		public string? UserNameCustomer { get; set; }
	}
}
