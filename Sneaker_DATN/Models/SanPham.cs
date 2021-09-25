using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class SanPham
    {
        [Key]
        public int ProductID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [Display(Name ="Tên sản phẩm")]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Vui lòng nhập giá"), Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        public double Price { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Hình ảnh 1")]
        public string? Image1 { get; set; }

        [Display(Name = "Hình ảnh 2")]
        public string? Image2 { get; set; }

        [ForeignKey("ThuongHieu")]
        [Display(Name = "Thương hiệu")]
        public int TradeMarkID { get; set; }

        [Display(Name = "Đang phục vụ")]
        public bool Status { get; set; }

        [Display(Name = "Mô tả")]
        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        public string? Description { get; set; }

        [NotMapped]
        [Display(Name ="Chọn hình")]
        public IFormFile ImageFile { get; set; }
    }
}
