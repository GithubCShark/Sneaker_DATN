using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sneaker_DATN.Models
{
    public class Size
    {
        [Display(Name = "Mã size")]
        public int SizeID { get; set; }

        [Display(Name = "Size")]
        public int Size1 { get; set; }
    }
}
