using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<QuanLy> QuanLys { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DonhangChitiet> DonhangChitiets { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<ThuongHieu> ThuongHieus { get; set; }
    }
}
