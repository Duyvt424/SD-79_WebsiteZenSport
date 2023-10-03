using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public string? Street { get; set; } // số đường
        public string? Commune { get; set; } // xã
        public string? District { get; set; } // huyện
        public string? Province { get; set; } // tỉnh
        public int Status { get; set; }
        public Guid CumstomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
