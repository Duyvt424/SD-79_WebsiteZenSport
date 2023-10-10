﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class ShoesDetails
    {
        public Guid ShoesDetailsId { get; set; }
        public string? ShoesDetailsCode { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        public decimal ImportPrice { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public Guid? ColorID { get; set; }
        public Guid? ProductID { get; set; }
        public Guid? SoleID { get; set; }
        public Guid? StyleID { get; set; }
        public string? ImageUrl { get; set; }
        public virtual Color Color { get; set; }
        public virtual Sole Sole { get; set; }
        public virtual Product Product { get; set; }
        public virtual Style Style { get; set; }
        public virtual List<Image> Images { get; set; }
        public virtual List<BillDetails> BillDetails { get; set; }
        public virtual List<ShoesDetails_Size> ShoesDetails_Size { get; set; }
        public virtual List<FavoriteShoes> FavoriteShoes { get; set; }
    }
}
