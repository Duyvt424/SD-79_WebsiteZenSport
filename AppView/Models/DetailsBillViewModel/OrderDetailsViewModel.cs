namespace AppView.Models.DetailsBillViewModel
{
	public class OrderDetailsViewModel
	{
        public Guid BillID { get; set; }
        public string BillCode { get; set; }
        public List<OrderStatusViewModel> OrderStatuses { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public Guid CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? PurchaseMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal? PriceVoucher { get; set; }
		public decimal? PriceVoucherShip { get; set; }
		public decimal ShippingCost { get; set; }
        public bool IsPaid { get; set; }
        public List<BillStatusHistoryViewModel> BillStatusHistories { get; set; }
        public List<AddressViewModel> AddressViewModels { get; set; }
    }
}