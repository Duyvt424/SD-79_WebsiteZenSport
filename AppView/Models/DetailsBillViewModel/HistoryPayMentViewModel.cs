namespace AppView.Models.DetailsBillViewModel
{
	public class HistoryPayMentViewModel
	{
		public Guid  ShoesDetailsID { get; set; }
		public string  ImageUrl { get; set; }
		public string  NameProduct { get; set; }
		public string Description { get; set; }
		public string Size { get; set; }
		public int QuantityReturned { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime PayMentDate { get; set; }
		public int TransactionType { get; set; }
		public string? PurChaseMethodName { get; set; }
		public int Status { get; set; }
		public string? Note { get; set; }
		public string? EmployeeName { get; set; }
    }
}
