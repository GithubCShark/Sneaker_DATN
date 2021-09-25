using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class DonHang
    {
        [Key]
        public int DonHangID { get; set; }

        [ForeignKey("KhachHang")]
        public int KhachHangID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt")]
        [Display(Name = "Ngày Đặt")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Ngaydat { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập tổng tiền")]
        [Display(Name ="Tổng tiền")]
        public double TongTien { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Địa chỉ giao")]
        public string DCGiao { get; set; }

        [Display(Name = "Tình trạng")]
        public string TinhTrang { get; set; }

        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public string Ghichu { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }
    }
}
