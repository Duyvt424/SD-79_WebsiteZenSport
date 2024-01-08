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
        public int Status { get; set; }
        public Guid BillId { get; set; }
        public Guid ShoesDetails_SizeID { get; set; }
        public Bill Bill { get; set; }
        public ShoesDetails_Size ShoesDetails_Size { get; set; }
    }
}
