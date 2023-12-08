using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class BillStatusHistory
    {
        public Guid BillStatusHistoryID { get; set; }
        public int Status { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Note { get; set; }
        public Guid BillID { get; set; }
        public Guid? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
