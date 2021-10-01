using Sneaker_DATN.Helpers;
using Sneaker_DATN.Models;
using Sneaker_DATN.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Services
{
    public interface IUserMemSvc
    {
        List<Users> GetAllUserMem();
        Users GetUserMem(int id);
        int AddUserMem(Users user);
        int EditUserMem(int id, Users user);
        public Users Login(ViewWebLogin viewWebLogin);
    }
    public class UserMemSvc : IUserMemSvc
    {
        protected DataContext _context;
        protected IEncodeHelper _encodeHelper;
        public UserMemSvc(DataContext context, IEncodeHelper encodeHelper)
        {
            _context = context;
            _encodeHelper = encodeHelper;
        }
        public int AddUserMem(Users user)
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

        public int EditUserMem(int id, Users user)
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

        

        public Users GetUserMem(int id)
        {
            Users user = null;
            user = _context.Users.Find(id);
            return user;
        }
        public List<Users> GetAllUserMem()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.Where(
                p => p.RoleID.Equals(3)
                ).ToList();
            return list;
        }
        public Users Login(ViewWebLogin viewWebLogin)
        {
            var u = _context.Users.Where(
                p => p.UserName.Equals(viewWebLogin.UserName)
                && p.Password.Equals(_encodeHelper.Encode(viewWebLogin.Password))
                && p.RoleID.Equals(3)
                ).FirstOrDefault();
            return u;
        }
    }
}
