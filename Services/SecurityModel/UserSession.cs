using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public class UserSession
    {
        public UserSession()
        {
            LoginTime = DateTime.UtcNow;
        }


        public UserSession(User user):this()
        {
            User = user;
        }


        public User User { get; set; }

        public DateTime LoginTime { get; set; }


    }
}
