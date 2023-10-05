using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class PurchaseMethod
    {
        public Guid PurchaseMethodID { get; set; }
        public string MethodName { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
