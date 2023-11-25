namespace AppView.Models
{
	public class AddressViewModel
	{
        public Guid AddressID { get; set; }
        public string? FullNameCus { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public bool IsDefaultAddress { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
