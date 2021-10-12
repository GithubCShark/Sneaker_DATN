using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IAdminSvc
    {
        List<Users> GetUserAll();

        Users GetUser(int id);

        public Users Login(ViewLogin viewLogin);
    }

    public class AdminSvc : IAdminSvc
    {
        protected DataContext _context;
        protected IEncodeHelper _encodeHelper;

        public AdminSvc(DataContext context, IEncodeHelper encodeHelper)
        {
            _context = context;
            _encodeHelper = encodeHelper;
        }

        public Users GetUser(int id)
        {
            Users user = null;
            user = _context.Users.Find(id);
            return user;
        }

        public List<Users> GetUserAll()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.ToList();
            return list;
        }

        public Users Login(ViewLogin viewLogin)
        {
            var u = _context.Users.Where(
                p => p.UserName.Equals(viewLogin.UserName)
                && p.Password.Equals(_encodeHelper.Encode(viewLogin.Password))
                && p.Lock.Equals(false)
                && (p.RoleID.Equals(1)
                || p.RoleID.Equals(2))
                ).FirstOrDefault();
            return u;
        }
    }
}
