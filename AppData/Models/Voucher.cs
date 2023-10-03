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
        public int MaxUsage { get; set; }
        public int RemainingUsage { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
