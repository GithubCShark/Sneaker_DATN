using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class ProductSize
    {
        public int ID { get; set; }
        public int IdSize { get; set; }

        [ForeignKey("ID")]
        public Products Products { set; get; }

        [ForeignKey("IdSize")]
        public Sizes Sizes { set; get; }
    }
}
