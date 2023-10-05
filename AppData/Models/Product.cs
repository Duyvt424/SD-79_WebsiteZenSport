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
        public string ProductCode { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid SupplierID { get; set; }
        public Guid MaterialId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Material Material { get; set; }
        public virtual List<ShoesDetails> ShoesDetails { get; set; }
    }
}
