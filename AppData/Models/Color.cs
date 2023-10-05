﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Color
    {
        public Guid ColorID { get; set; }
        public string ColorCode { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual List<ShoesDetails> ShoesDetails { get; set; }
    }
}
