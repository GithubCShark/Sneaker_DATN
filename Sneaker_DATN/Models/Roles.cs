using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }

        [Display(Name = "Quyền")]
        public string Role { get; set; }
    }
}
