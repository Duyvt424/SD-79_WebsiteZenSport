namespace AppView.Models
{
    public class BillViewModel
    {
        public Guid BillID { get; set; }
        public string BillCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime SuccessDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingCosts { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public string CustomerName { get; set; }
        public string VoucherName { get; set; }
        public string EmployeeName { get; set; }
        public string PurchaseMethodName { get; set; }
    }
}
