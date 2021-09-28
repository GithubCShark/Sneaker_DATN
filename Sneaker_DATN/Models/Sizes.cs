using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sneaker_DATN.Models
{
    public class Sizes
    {
        [Key]
        [Display(Name = "Mã size")]
        public int SizeID { get; set; }

        public int Size { get; set; }
    }
}
