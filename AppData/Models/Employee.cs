using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Employee
    {
        public Guid EmployeeID { get; set; }
        public string? Image { get; set; }
        public string? IdentificationCode { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Sex { get; set; }
        public string? Address { get; set; }
        public string? ResetPassword { get; set; }//\
        public string? PhoneNumber { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid RoleID { get; set; }
        public virtual Role Role { get; set; }
        public virtual List<Bill> Bills { get; set; }
        public virtual List<BillStatusHistory> BillStatusHistories { get; set; }
    }
}
