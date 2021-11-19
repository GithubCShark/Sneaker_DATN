using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class ViewDashBoard
    {
        public Orders Orders { get; set; }

        [Display(Name = "Thương hiệu")]
        public string BrandName { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
    }
}
