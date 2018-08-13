using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityModel
{
    public abstract class UserGroup
    {
        public UserGroup()
        {
        }

        public string UserGroupName { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<User> Users { get; }
        public ICollection<Role> Roles { get; }


        public override bool Equals(object obj)
        {
            return IsEnabled
                && UserGroupName.Equals(
                    obj.ToString(),
                    StringComparison.InvariantCultureIgnoreCase);
        }


        public override string ToString()
        {
            return UserGroupName;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public static bool operator ==(UserGroup userGroup, string userGroupName)
        {
            return userGroup != null
                && userGroup.Equals(userGroupName);
        }


        public static bool operator !=(UserGroup userGroup, string userGroupName)
        {
            return !(userGroup == userGroupName);
        }


        public static bool operator &(UserGroup userGroup, string permissionCode)
        {
            return userGroup != null
                && userGroup.HasPermission(permissionCode);
        }


        public bool HasPermission(string permissionCode)
        {
            return Roles.Any(r => r.HasPermission(permissionCode));
        }
    }
}
