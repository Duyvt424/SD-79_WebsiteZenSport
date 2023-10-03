using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public virtual List<ShoesDetails> ShoesDetails { get; set; }
    }
}
