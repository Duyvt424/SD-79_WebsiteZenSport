using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class ShoesDetails_Size
    {
        public Guid ID { get; set; }
        public int Quantity { get; set; }
        public Guid ShoesDetailsId { get; set; }
        public Guid SizeID { get; set; }
        public virtual ShoesDetails ShoesDetails { get; set; }
        public virtual Size Size { get; set; }
        public virtual List<CartDetails> CartDetails { get; set; }
    }
}
