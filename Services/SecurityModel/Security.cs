using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public abstract class Security
    {
        public Security()
        {
        }

        
        public IEnumerable<Role> Roles { get; protected set; }


        /// <summary>
        /// Gets an existing user object associated with user name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public abstract User GetUser(string userName);


        /// <summary>
        /// Creates a new user associated with user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public abstract User NewUser(string userName);


        public UserSession Login(string userName, string password)
        {
            var user = GetUser(userName);

            return user != null && user.CheckPassword(password) ?
                new UserSession(user) :
                null;
        }
    }
}
