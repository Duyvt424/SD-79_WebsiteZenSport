using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Voucher
    {
        public Guid VoucherID { get; set; }
        public string? VoucherCode { get; set; }
        public decimal VoucherValue { get; set; }
		public decimal Total { get; set; }
		public string? Exclusiveright { get; set; }

		public int MaxUsage { get; set; }
		public int? Type { get; set; }
		public int RemainingUsage { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
		public bool IsDel { get; set; }
		public DateTime DateCreated { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
