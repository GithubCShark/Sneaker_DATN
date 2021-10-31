using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
namespace Sneaker_DATN.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name ="Tài Khoản")]
        [Required(ErrorMessage ="Vui lòng nhập tài khoản")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Giới tính")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string ImgUser { get; set; }

        [NotMapped]
        [Display(Name = "Chọn hình")]
        public IFormFile ImageUser { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Vui lòng nhập đúng đinh dạng là SĐT")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ")]
        [Column(TypeName = "nvarchar(250)"), MaxLength(250)]
        public string Address { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? DOB { get; set; }

        [Display(Name ="Mật khẩu")]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set;}

        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Khóa")]
        public bool Lock { get; set; }

        [ForeignKey("Roles")]
        [Display(Name = "Quyền")]
        public int RoleID { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateCreated { get; set; }

        public Roles Roles { get; set; }
    }
}
