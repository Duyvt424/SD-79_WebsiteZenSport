using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Size
    {
        public Guid SizeID { get; set; }
        public string SizeCode { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<ShoesDetails_Size> ShoesDetails_Size { get; set; }
    }
}
