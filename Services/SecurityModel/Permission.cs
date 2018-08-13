using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityModel
{
    /// <summary>
    /// Permission codes defined in program code
    /// </summary>
    public abstract class Permission
    {
        public Permission()
        {
        }


        public string PermissionCode { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }


        public ICollection<SecuredModule> Modules { get; set; }

        public ICollection<SecuredAction> Actions { get; set; }

        public ICollection<Role> Roles { get; set; }


        public override bool Equals(object obj)
        {
            return PermissionCode.Equals(
                    obj.ToString(), 
                    StringComparison.InvariantCultureIgnoreCase);
        }


        public override string ToString()
        {
            return PermissionCode;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public static bool operator ==(Permission permission, string permissionCode)
        {
            return permission != null
                && permission.Equals(permissionCode);
        }


        public static bool operator !=(Permission permission, string otherCode)
        {
            return !(permission == otherCode);
        }


        /// <summary>
        /// Returns permission usage info
        /// </summary>
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public IEnumerable<User> Users
        {
            get
            {
                return Roles
                       .Where(r => r.IsEnabled)
                       .SelectMany(r =>
                            r.UserGroups
                            .Where(g => g.IsEnabled)
                            .SelectMany(g => 
                                g.Users
                                .Where(u => u.IsEnabled)));
            }
        }

    }
}
