using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models.ViewModels
{
    public class ViewWebLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleID { get; set; }

        public string ReturnUrl { get; set; }
    }
}
