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
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        //[Column(TypeName = "nvarchar(100)")]
        //[Display(Name = "Chức danh")]
        //[Required(ErrorMessage = "Vui lòng nhập chức danh")]
        //public string Title { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Vui lòng nhập đúng đinh dạng là SĐT")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? DOB { get; set; }

        [Display(Name ="Mật khẩu")]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set;}

        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [ForeignKey("Roles")]
        [Display(Name = "Quyền")]
        public int RoleID { get; set; }

        public Roles Roles { get; set; }
    }
}
