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

        public int ID { get; set; }
        public int ColorID { get; set; }

        [ForeignKey("ID")]
        public Products Products { get; set; }
        [ForeignKey("ColorID")]
        public Colors Colors { get; set; }
    }
}
