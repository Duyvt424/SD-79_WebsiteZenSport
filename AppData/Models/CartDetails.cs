using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class CartDetails
    {
        public Guid CartDetailsId { get; set; }
        public Guid CumstomerID { get; set; }
        public Guid ShoesDetailsId { get; set; }
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ShoesDetails ShoesDetails { get; set; }
    }
}
