using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Material
    {
        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
