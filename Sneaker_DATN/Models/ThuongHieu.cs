using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sneaker_DATN.Models
{
    public class ThuongHieu
    {
        [Key]
        [Display(Name = "Mã thương hiệu")]
        public int MaTH { get; set; }

        [Display(Name = "Tên thương hiệu")]
        public string TenTH { get; set; }
    }
}
