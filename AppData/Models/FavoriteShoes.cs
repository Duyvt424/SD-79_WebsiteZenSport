using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class FavoriteShoes
    {
        public Guid FavoriteID { get; set; }
        public Guid ShoesDetailsId { get; set; }
        public Guid CumstomerID { get; set; }
        public virtual ShoesDetails ShoesDetails { get; set; }
        public virtual Customer Customer { get; set; }
        
    }
}
