using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
	public class ReturnedProducts
	{
		public Guid ID { get; set; }
		public DateTime CreateDate { get; set; }
		public string Note { get; set; }
		public int QuantityReturned { get; set; }
        public decimal ReturnedPrice { get; set; }
        public int TransactionType { get; set; }
        public string NamePurChaseMethod { get; set; }
        public decimal ShippingFeeReturned { get; set; } // Tiền ship hoàn trả
        public decimal InitialProductTotalPrice { get; set; } // Tổng tiền sản phẩm ban đầu
        public int Status { get; set; }
        public Guid BillId { get; set; }
        public Guid? ShoesDetails_SizeID { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual ShoesDetails_Size ShoesDetails_Size { get; set; }
    }
}
