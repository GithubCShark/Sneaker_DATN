using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sneaker_DATN.Models
{
    public class Brands
    {
        [Key]
        [Display(Name = "Mã thương hiệu")]
        public int BrandID { get; set; }

        [Display(Name = "Tên thương hiệu")]
        public string BrandName { get; set; }
    }
}
