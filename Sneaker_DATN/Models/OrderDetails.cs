using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class OrderDetails
    {
        [Key]
        public int DetailID { get; set; }

        [ForeignKey("Orders")]
        public int OrderID { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn giá")]
        public double Price { get; set; }

        [ForeignKey("Sizes")]
        [Display(Name = "Mã size")]
        public int SizeID { get; set; }

        [ForeignKey("Colors")]
        [Display(Name = "Mã màu")]
        public int ColorID { get; set; }

        public Orders Orders { get; set; }

        public Products Products { get; set; }

        public Sizes Sizes { get; set; }

        public Colors Colors { get; set; }
    }
}
