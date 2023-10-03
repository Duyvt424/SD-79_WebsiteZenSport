using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Cart
    {
        public Guid CumstomerID { get; set; }
        public string Description { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<CartDetails> CartDetails { get; set; }
    }
}
