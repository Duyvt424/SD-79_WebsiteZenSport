namespace AppView.Models.DashBoardViewModel
{
	public class TopVoucherStatisticsViewModel
	{
		public Guid VoucherID { get; set; }
		public string VoucherCode { get; set; }
		public int TotalUsage { get; set; }
		public decimal VoucherValue { get; set; }
	}
}
