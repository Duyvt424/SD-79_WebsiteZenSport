using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Sole
    {
        public Guid SoleID { get; set; }
        public string SoleCode { get; set; }
        public string? Name { get; set; }
        public int Height { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<ShoesDetails> ShoesDetails { get; set; }
    }
}
