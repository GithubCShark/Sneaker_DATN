using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        [Display(Name ="Tên sản phẩm")]
        public string ProductName { get; set; }

        [Column(TypeName = "money")]
        [Required(ErrorMessage = "Vui lòng nhập giá"), Range(0, double.MaxValue, ErrorMessage ="Vui lòng nhập giá")]
        [Display(Name = "Giá")]
        public double Price { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giảm giá")]
        public double Sale { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Hình ảnh 1")]
        public string Image1 { get; set; }

        [Display(Name = "Hình ảnh 2")]
        public string Image2 { get; set; }

        [ForeignKey("Brands")]
        [Display(Name = "Thương hiệu")]
        public int BrandID { get; set; }

        [Display(Name = "Đang phục vụ")]
        public bool Status { get; set; }

        [Display(Name = "Mô tả")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [NotMapped]
        [Display(Name ="Chọn hình")]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        [Display(Name = "Chọn hình 1")]
        public IFormFile ImageFile1 { get; set; }

        [NotMapped]
        [Display(Name = "Chọn hình 2")]
        public IFormFile ImageFile2 { get; set; }

        public Brands Brands { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}
