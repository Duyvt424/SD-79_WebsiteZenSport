using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class ShippingVoucher
	{
		public Guid ShippingVoucherID { get; set; }
		public string? VoucherShipCode { get; set; }
	
		public decimal? MaxShippingDiscount { get; set; } // Giảm giá tối đa cho vận chuyển
		public decimal? ShippingDiscount { get; set; } // Giảm giá cụ thể cho vận chuyển
		public int QuantityShip { get; set; }
		public DateTime ExpirationDate { get; set; }
		public int IsShippingVoucher { get; set; }
		public DateTime DateCreated { get; set; }
		public virtual List<Bill> Bills { get; set; }
	}
}
