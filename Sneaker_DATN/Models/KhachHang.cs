using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class KhachHang
    {
        [Key]
        public int KhanhHangId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        [Required(ErrorMessage ="Vui lòng nhập tên")]
        [Display(Name ="Họ và Tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [Display(Name = "Ngày Sinh")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime NgaySinh { get; set; }
        [Column(TypeName = "varchar(15)"), MaxLength(15)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^\(?([0-9]{3})[-. ]?([0-9]{4})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
        //091-1234-567
        public string PhoneNumber { get; set; }
        [Column(TypeName = "varchar(50)"),MaxLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu"), Display(Name = "Mật Khẩu")]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="Vui lòng xác nhận mật khẩu"), Display(Name ="Xác nhận mật khẩu")]
        [Column(TypeName = "varchar(50)"), MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        [StringLength(250)]
        [Display(Name ="Mô tả")]
        public string? Mota { get; set; }
    }
}
