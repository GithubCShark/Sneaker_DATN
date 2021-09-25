using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Sneaker_DATN.Models
{
    public class MauSac
    {
        [Display(Name = "Mã màu")]
        public int ColorID { get; set; }

        [Display(Name = "Màu sắc")]
        public string Color { get; set; }
    }
}
