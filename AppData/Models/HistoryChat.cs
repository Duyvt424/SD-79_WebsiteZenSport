using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class HistoryChat
    {
        public Guid HistoryChatID { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
