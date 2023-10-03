using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Customer
    {
        public Guid CumstomerID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Sex { get; set; }
        public string? ResetPassword { get; set; }//
        public string? PhoneNumber { get; set; }
        public int Status { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual List<Bill> Bills { get; set; }
        public virtual List<Address> Addresses { get; set; }
    }
}
