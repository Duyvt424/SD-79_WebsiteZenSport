namespace AppView.Models
{
	public class ShoppingCartViewModel
	{
		public List<CartItemViewModel> CartItems { get; set; }
		public List<AddressViewModel> AddressList { get; set; }
		public List<VoucherViewModel> Vouchers { get; set; }
	}
}

