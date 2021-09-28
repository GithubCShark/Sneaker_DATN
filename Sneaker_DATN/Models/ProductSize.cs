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
        [Key]
        [ForeignKey("Products")]
        public int ProductID { get; set; }

        [ForeignKey("Sizes")]
        public int SizeID { get; set; }

        public Products Products { get; set; }

        public Sizes Sizes { get; set; }
    }
}
