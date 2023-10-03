using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Coupon
    {
        public Guid CouponID { get; set; }
        public string? CouponCode { get; set; }
        public decimal CouponValue { get; set; }
        public int MaxUsage { get; set; }
        public int RemainingUsage { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
