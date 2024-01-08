using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Bill
    {
        public Guid BillID { get; set; }
        public string BillCode { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime SuccessDate { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PaymentDay { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingCosts { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public string Note { get; set; }
        public bool IsPaid { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? VoucherID { get; set; }
		public Guid? ShippingVoucherID { get; set; }
		public Guid? EmployeeID { get; set; }
        public Guid PurchaseMethodID { get; set; }
        public Guid? AddressID { get; set; }
        public virtual PurchaseMethod PurchaseMethod { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Voucher Voucher { get; set; }
		public virtual ShippingVoucher ShippingVoucher { get; set; }
		public virtual Customer Customer { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<BillDetails> BillDetails { get; set; }
        public virtual List<BillStatusHistory> BillStatusHistories { get; set; }
        public virtual List<ReturnedProducts> ReturnedProducts { get; set; }
    }
}
