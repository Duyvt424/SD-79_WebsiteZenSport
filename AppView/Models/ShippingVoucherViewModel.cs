namespace AppView.Models
{
    public class ShippingVoucherViewModel
    {
        public Guid ShippingVoucherID { get; set; }
        public string? VoucherShipCode { get; set; }

        public decimal? MaxShippingDiscount { get; set; } // Giảm giá tối đa cho vận chuyển
        public decimal? ShippingDiscount { get; set; } // Giảm giá cụ thể cho vận chuyển
        public int QuantityShip { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int IsShippingVoucher { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
