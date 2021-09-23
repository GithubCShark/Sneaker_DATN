using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class QuanLy
    {
        [Key]
        public int QuanLyId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name ="Tài Khoản")]
        [Required(ErrorMessage ="Vui lòng nhập tài khoản")]
        public string UserName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Chức danh")]
        [Required(ErrorMessage = "Vui lòng nhập chức danh")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? DOB { get; set; }
        [Display(Name ="Quản trị")]
        public bool Admin { get; set; }
        [Display(Name ="Sử dụng")]
        public string Password { get; set;}
        [Column(TypeName = "nvarchar(50)"), MaxLength(50)]
        [Display(Name = "Xác nhận Mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
