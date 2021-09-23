using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public enum TrangthaiDonhang
    {
        [Display(Name ="Mới đặt")]
        Moidat = 1,
        [Display(Name ="Đang giao")]
        Danggiao = 2,
        [Display(Name ="Đã giao")]
        Dagiao = 3
    }
    public class DonHang
    {
        [Key]
        public int DonHangId { get; set; }
        [ForeignKey("KhachHang")]
        public int KhachHangId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày đặt")]
        [Display(Name = "Ngày Đặt")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Ngaydat { get; set; }
        [Required, Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập tổng tiền")]
        [Display(Name ="Tổng tiền")]
        public double TongTien { get; set; }
        [Display(Name ="Trạng thái")]
        public TrangthaiDonhang TrangthaiDonhang { get; set; }
        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public string Ghichu { get; set; }
        public KhachHang KhachHang { get; set; }
        public List<DonhangChitiet> DonhangChitiets { get; set; }
    }
}
