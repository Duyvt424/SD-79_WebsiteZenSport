using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class ShoesDetails
    {
        public Guid ShoesDetailsId { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Guid? ColorID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? SizeID { get; set; }
        public Guid? SoleID { get; set; }
        public Guid? StyleID { get; set; }
        public Guid? SupplierID { get; set; }
        public string? ImageUrl { get; set; }
        public virtual Color Color { get; set; }
        public virtual Size Size { get; set; }
        public virtual Sole Sole { get; set; }
        public virtual Product Product { get; set; }
        public virtual Style Style { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<CartDetails> CartDetails { get; set; }
        public virtual List<BillDetails> BillDetails { get; set; }
    }
}
