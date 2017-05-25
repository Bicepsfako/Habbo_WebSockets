using System;

namespace Wandala.HabboHotel.Users.UserData
{
    public class UserDataNotFoundException : Exception
    {
        public UserDataNotFoundException(string reason)
            : base(reason)
        {
        }
    }
}