using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sneaker_DATN.Constant
{
    public class SessionKey
    {
        public static class User
        {
            public const string UserName = "UserName";
            public const string FullName = "FullName";
            public const string Valid = "Valid";
            public const string UserContext = "UserContext";
        }

        public static class Guest
        {
            public const string Guest_Email = "KH_Email";
            public const string Guest_FullName = "KH_FullName";
            public const string Valid = "Valid";
            public const string GuestContext = "GuestContext";
        }
    }
}
