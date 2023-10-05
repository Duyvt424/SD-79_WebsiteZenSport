using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Rank
    {
        public Guid RankID { get; set; }
        public string RankCode { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public decimal ThresholdAmount { get; set; }
        public decimal ReducedValue { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<Customer> Customers { get; set; }
    }
}
