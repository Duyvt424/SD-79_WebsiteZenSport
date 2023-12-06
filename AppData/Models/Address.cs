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
        public bool IsDefaultAddress { get; set; }
        public decimal ShippingCost { get; set; }
        public int DistrictId { get; set; }
        public int WardCode { get; set; }
        public int ShippingMethodID { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CumstomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
