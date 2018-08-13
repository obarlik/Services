using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public class UserSession
    {
        public UserSession()
        {
        }


        public UserSession(User user) : this()
        {
            User = user;
            LoginTime = DateTime.UtcNow;
        }


        public User User { get; set; }

        public DateTime LoginTime { get; set; }
    }
}
