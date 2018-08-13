using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityModel
{
    public abstract class Role
    {
        public Role()
        {
        }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }


        /// <summary>
        /// Gets a collection of permissions given to this role
        /// </summary>
        public ICollection<Permission> Permissions { get; }


        /// <summary>
        /// Gets a collection of user groups in which this role belongs to
        /// </summary>
        public ICollection<UserGroup> UserGroups { get; }


        public bool HasPermission(string permissionCode)
        {
            return Permissions.Any(p => p == permissionCode);
        }


        public override bool Equals(object obj)
        {
            return RoleName.Equals(
                    obj.ToString(), 
                    StringComparison.InvariantCultureIgnoreCase);
        }


        public override string ToString()
        {
            return RoleName;
        }


        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public static bool operator ==(Role role, string roleName)
        {
            return role != null 
                && role.Equals(roleName);
        }


        public static bool operator !=(Role role, string roleName)
        {
            return !(role == roleName);
        }


        public static bool operator &(Role role, string permissionCode)
        {
            return role != null
                && role.HasPermission(permissionCode);
        }


        /// <summary>
        /// Returns permission usage info
        /// </summary>
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public IEnumerable<PermissionUsage> Usage
        {
            get
            {
                return UserGroups.SelectMany(g =>
                        g.Users.Select(u =>
                            new PermissionUsage()
                            {
                                User = u,
                                UserGroup = g,
                                Role = this
                            }));
            }
        }

    }
}
