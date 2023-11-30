namespace AppView.Models.DetailsBillViewModel
{
	public class OrderDetailsViewModel
	{
        public string BillCode { get; set; }
        public List<OrderStatusViewModel> OrderStatuses { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? PurchaseMethod { get; set; }
    }
}