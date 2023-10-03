using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class BillDetails
    {
        public Guid ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public Guid ShoesDetailsId { get; set; }
        public Guid BillID { get; set; }
        public virtual ShoesDetails ShoesDetails { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
