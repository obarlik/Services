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


        public bool IsEnabled { get; set; }


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
        /// <param name="permissionCode"></param>
        /// <returns></returns>
        public bool HasPermission(string permissionCode)
        {
            return GetPermissionsCodes()
                   .Contains(permissionCode);
        }


        Dictionary<string, bool> _ActionPermissions;


        /// <summary>
        /// Check if user has permisson for a module or module's action
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool HasPermissionForAction(string moduleName, string actionName = null)
        {
            // Creating key for action permission
            var key = string.Format(
                        "Action[{0}{1}]",
                        moduleName,
                        actionName == null ?
                            "" :
                            ("." + actionName));

            // Creating action permission cache if not exists
            if (_ActionPermissions == null)
            {
                lock (this)
                {
                    if (_ActionPermissions == null)
                        _ActionPermissions = new Dictionary<string, bool>();
                }
            }


            bool result;

            // Trying to get action permission from cache if exists
            if (_ActionPermissions.TryGetValue(key, out result))
                return result;

            lock (_ActionPermissions)
            {
                if (_ActionPermissions.TryGetValue(key, out result))
                    return result;
                
                // We need to update cache for action permission

                // Searching module by name
                var module =
                    SecuritySystem.Instance
                    .SecuredModules
                    .AsParallel()
                    .FirstOrDefault(m => m.Name == moduleName);

                // Checking if module exists and has permission
                result = module == null
                      || (module.IsEnabled
                       && (module.Permission == null
                        || HasPermission(module.Permission.PermissionCode)));

                if (result 
                 && module != null 
                 && actionName != null)
                {
                    // If module permission check passes
                    // we need to check action permissions too

                    // Searching action by name
                    var action =
                        module.Actions
                        .AsParallel()
                        .FirstOrDefault(a => a.Name == actionName);

                    // Checking if action exists and has permission
                    result = action == null
                          || (action.IsEnabled 
                           && (action.Permission == null 
                            || HasPermission(action.Permission.PermissionCode)));
                }

                _ActionPermissions[key] = result;

                return result;
            }
        }


        HashSet<string> _PermissionCodes;

        /// <summary>
        /// Returns all permissions for user
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetPermissionsCodes()
        {
            return _PermissionCodes ??
                  (_PermissionCodes =
                   new HashSet<string>(
                       GetPermissions()
                       .Select(p => p.PermissionCode)));
        }
        

        List<Permission> _Permissions;

        /// <summary>
        /// Returns all permissions for user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermissions()
        {
            return _Permissions ??
                  (_Permissions =
                       UserGroups
                       .Where(g => g.IsEnabled)
                       .SelectMany(u =>
                            u.Roles
                            .Where(r => r.IsEnabled)
                            .SelectMany(r =>
                                r.Permissions
                                .Where(p => p.IsEnabled)))
                       .ToList());
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
    }
}
