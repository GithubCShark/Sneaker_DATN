using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class SaleOff
    {
        [Key]
        public int SaleID { get; set; }

        [Display(Name = "Phần trăm giảm")]
        public double Percent { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        public int ID { get; set; }
        [ForeignKey("ID")]
        public Products Products { get; set; }
    }
}
