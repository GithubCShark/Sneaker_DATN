using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class DonhangChitiet
    {
        [Key]
        [ForeignKey("DonHang")]
        public int DonHangID { get; set; }

        [ForeignKey("SanPham")]
        public int SanPhamID { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Vui lòng nhập số lượng")]
        [Display(Name = "Số lượng")]
        public int Soluong { get; set; }

        [Display(Name = "Đơn giá")]
        public Nullable<decimal> DonGia { get; set; }

        [Display(Name = "Mã size")]
        public int MaSize { get; set; }

        [Display(Name = "Mã màu")]
        public int MaMau { get; set; }
    }
}
