using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Role
    {
        public Guid RoleID { get; set; }
        public string? RoleCode { get; set; }
        public string? RoleName { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
