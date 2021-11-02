using System;
using Sneaker_DATN.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    //public interface IChartSvc
    //{
    //    int GetDateAllUserMem();
    //    //List<Orders> GetDateAllOrder(DateTime year);
    //}
    //public class ChartSvc : IChartSvc
    //{
    //    protected DataContext _context;
    //    public ChartSvc(DataContext dataContext)
    //    {
    //        _context = dataContext;
    //    }

    //    public List<> GetDateAllUserMem()
    //    {
    //        List<Users> list = new List<Users>();
    //        list = _context.Users.ToList();
    //        var listcount = from p in list
    //                        where p.DateCreated.Value.Year == 2021
    //                        group p by p.DateCreated.Value.Month into List
    //                        select new
    //                        {
    //                            Thang = List.Key,
    //                            SoLuong = List.Count()
    //                        };
    //        return listcount;
    //    }
    //}
}
