using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class ViewCart
    {
        public Products Products { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }

    }
}
