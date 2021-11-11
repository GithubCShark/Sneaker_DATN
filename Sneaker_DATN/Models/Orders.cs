using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [Display(Name = "Ngày Đặt")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreate { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập tổng tiền")]
        [Display(Name ="Tổng tiền")]
        public double Total { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Địa chỉ giao")]
        public string Address { get; set; }

        [Display(Name = "Tình trạng")]
        public string Status { get; set; }

        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        public bool ExpDiscount { get; set; }

        public string VoucherCode { get; set; }

        public int VoucherId { get; set; }

        [Display(Name = "Số tiền thanh toán")]
        public double PaymentAmount { get; set; }

        public Users Users { get; set; }
    }
}
