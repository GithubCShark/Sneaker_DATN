using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IUserSvc
    {
        List<Users> GetAllUser();
        Users GetUser(int id);
        int AddUser(Users user);
        int EditUser(int id, Users user);
        public Users Login(ViewWebLogin viewWebLogin);
    }
    public class UserSvc : IUserSvc
    {
        protected DataContext _context;
        protected IEncodeHelper _encodeHelper;
        public UserSvc(DataContext context, IEncodeHelper encodeHelper)
        {
            _context = context;
            _encodeHelper = encodeHelper;
        }
        public int AddUser(Users user)
        {
            int ret = 0;
            try
            {
                user.Password = _encodeHelper.Encode(user.Password);
                user.ConfirmPassword = user.Password;
                _context.Add(user);
                _context.SaveChanges();
                ret = user.UserID;
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }

        public int EditUser(int id, Users user)
        {
            int ret = 0;
            try
            {
                Users _user = null;
                _user.UserName = user.UserName;
                _user.FullName = user.FullName;
                _user.Gender = user.Gender;
                _user.Email = user.Email;
                _user.PhoneNumber = user.PhoneNumber;
                _user.DOB = user.DOB;
                if (user.Password != null)
                {
                    user.Password = _encodeHelper.Encode(user.Password);
                    _user.Password = user.Password;
                }
                _user.RoleID = user.RoleID;
                _context.Update(_user);
                _context.SaveChanges();
                ret = user.UserID;
            }
            catch (Exception)
            {
                ret = 0;
            }
            return ret;
        }

        public List<Users> GetAllUser()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.ToList();
            return list;
        }

        public Users GetUser(int id)
        {
            Users user = null;
            user = _context.Users.Find(id);
            return user;
        }
        public Users Login(ViewWebLogin viewWebLogin)
        {
            var u = _context.Users.Where(
                p => p.UserName.Equals(viewWebLogin.UserName)
                && p.Password.Equals(_encodeHelper.Encode(viewWebLogin.Password))
                ).FirstOrDefault();
            return u;
        }
    }
}
