namespace AppView.Models.DashBoardViewModel
{
	public class tablesViewModel
	{
		public Guid BillID { get; set; }
		public string BillCode { get; set; }
		public int TotalShoes { get; set; }
		public decimal Price { get; set; }
		public Guid CustomerID { get; set; }
		public string FullNameCus { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime CreateDate { get; set; }
		public string PurchasePayMent { get; set; }
		public int Status { get; set; }
	}
}
