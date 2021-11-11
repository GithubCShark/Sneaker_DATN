using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class Discounts
    {
        [Key]
        public int VoucherId { get; set; }
        [Display(Name = "Mã voucher")]

        [StringLength(50)]
        public string VoucherCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? DayStart { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime? DayEnd { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày sử dụng")]
        public DateTime DateUse { get; set; }

        [Display(Name = "Khách hàng sử dụng")]
        public int CustomerId { get; set; }

        [Display(Name = "Khách hàng sử dụng")]
        public bool Status { get; set; }

        [Display(Name = "Số tiền giảm")]
        public bool Classify { get; set; }

        [Display(Name = "Số tiền giảm")]
        public double Price { get; set; }

        [Display(Name = "Phần trăm giảm")]
        public double Percent { get; set; }
    }
}
