using System;
using System.Linq;
using System.Collections.Generic;

namespace SecurityModel
{
    /// <summary>
    /// User object for security subsystem
    /// </summary>
    public abstract class User
    {
        public User()
        {
        }

        /// <summary>
        /// User's login name
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// User's email address for communication
        /// </summary>
        public string Email { get; set; }



        public override bool Equals(object obj)
        {
            return UserName.Equals(
                obj.ToString(),
                StringComparison.InvariantCultureIgnoreCase);
        }


        public override string ToString()
        {
            return UserName;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        /// Checks if password is ok for this user.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public abstract bool CheckPassword(string password);


        /// <summary>
        /// Sets a password for this user
        /// </summary>
        /// <param name="newPassword"></param>
        public abstract void SetPassword(string newPassword);


        /// <summary>
        /// Gets a collection of user groups in which this user belongs to
        /// </summary>
        public abstract ICollection<UserGroup> UserGroups { get; }
        

        /// <summary>
        /// Checks if user has permission or not
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public bool HasPermission(string permissionCode)
        {
            return UserGroups.Any(g => g & permissionCode);
        }


        public static bool operator ==(User user, string userName)
        {
            return user != null
                && user.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
        }


        public static bool operator !=(User user, string userName)
        {
            return !(user == userName);
        }


        public static bool operator &(User user, string permissionCode)
        {
            return user != null
                && user.HasPermission(permissionCode);
        }
    }
}
