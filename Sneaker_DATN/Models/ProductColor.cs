using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class ProductColor
    {
        [Key]
        [ForeignKey("Products")]
        public int ProductID { get; set; }

        [ForeignKey("Colors")]
        public int ColorID { get; set; }

        public Products Products { get; set; }

        public Colors Colors { get; set; }
    }
}
