using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class SizeSP
    {
        [Key]
        [ForeignKey("SanPham")]
        public int ProductID { get; set; }
        [ForeignKey("Size")]
        public int SizeID { get; set; }
    }
}
