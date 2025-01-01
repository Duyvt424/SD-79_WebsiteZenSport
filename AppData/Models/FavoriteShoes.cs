using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class FavoriteShoes
    {
        public Guid FavoriteShoesID { get; set; }
        public Guid ShoesDetails_SizeId { get; set; }
        public Guid CumstomerID { get; set; }
        public DateTime AddedDate { get; set; }
        public int Status { get; set; }
        public virtual ShoesDetails_Size ShoesDetails_Size { get; set; }
        public virtual Customer Customer { get; set; }
        
    }
}
