using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public enum PhanLoai
    {
        [Display(Name = "Adidas")]
        Adidas = 1,
        [Display(Name = "Nike")]
        Nike = 2,
        [Display(Name = "Converse")]
        Converse = 3
    }
    public class SanPham
    {
        [Key]
        public int SanPhamId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [Display(Name ="Tên sản phẩm")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        public string? MoTa { get; set; }
        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Vui lòng nhập giá"), Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        public double Gia { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn"), Range(1, double.MaxValue, ErrorMessage = "Vui lòng chọn")]
        [Display(Name = "Phân loại")]
        public PhanLoai PhanLoai { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        [StringLength(150)]
        [Display(Name = "Hình ảnh")]
        public string? HinhAnh { get; set; }
        [Display(Name = "Đang phục vụ")]
        public bool TrangThai { get; set; }
        [NotMapped]
        [Display(Name ="Chọn hình")]
        public IFormFile ImageFile { get; set; }
    }
}
