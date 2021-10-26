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
        Roles GetRole(int id);
        List<Users> GetAllUser();
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
                _user = _context.Users.Find(id);

                _user.FullName = user.FullName;
                _user.Gender = user.Gender;
                _user.Email = user.Email;
                _user.PhoneNumber = user.PhoneNumber;
                _user.Address = user.Address;
                _user.DOB = user.DOB;
                _user.ImgUser = user.ImgUser;
                _user.Lock = user.Lock;
                _user.RoleID = user.RoleID;
                if (user.Password != null)
                {
                    user.Password = _encodeHelper.Encode(user.Password);
                    _user.Password = user.Password;
                    _user.ConfirmPassword = user.Password;
                }
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

        public Roles GetRole(int id)
        {
            Roles roles = null;
            roles = _context.Roles.Find(id);
            return roles;
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
        public List<Users> GetAllUser()
        {
            List<Users> list = new List<Users>();
            list = _context.Users.ToList();
            return list;
        }
        public Users Login(ViewWebLogin viewWebLogin)
        {
            var u = _context.Users.Where(
                p => p.UserName.Equals(viewWebLogin.UserName)
                && p.Password.Equals(_encodeHelper.Encode(viewWebLogin.Password))
                && p.RoleID.Equals(3)
                && p.Lock.Equals(false)
                ).FirstOrDefault();
            return u;
        }
    }
}
