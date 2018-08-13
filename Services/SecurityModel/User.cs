using System;
using System.Linq;
using System.Collections.Generic;
using Services.Utilities;

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


        DictionaryCache<bool> _ModulePermissions;
        DictionaryCache<bool> _ActionPermissions;

        
        /// <summary>
        /// Check if user has permisson for a module
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public bool HasPermissionForModule(string moduleName)
        {
            // Creating key for action permission
            var key = string.Format(
                        "Module[{0}]",
                        moduleName);

            return this.LockedResult(
                criteria: 
                    () => _ModulePermissions == null,
                locked: 
                    () => _ModulePermissions = 
                        new DictionaryCache<bool>(
                            keyText =>
                            {
                                // Searching module by name
                                var module =
                                    SecuritySystem.Instance
                                    .SecuredModules
                                    .AsParallel()
                                    .FirstOrDefault(m => m.Name == keyText);

                                // Checking if module exists and this user has permission
                                return module == null
                                    || module.CheckUserPermission(this);
                            }),
                unlocked: 
                    () => _ModulePermissions)[key];          
        }



        /// <summary>
        /// Check if user has permisson for a module's action
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool HasPermissionForAction(string moduleName, string actionName)
        {
            // Creating key for action permission
            var key = string.Format(
                        "Action[{0}.{1}]",
                        moduleName,
                        actionName);

            return this.LockedResult(
                criteria: 
                    () => _ActionPermissions == null,
                locked: 
                    () => _ActionPermissions =
                        new DictionaryCache<bool>(
                            keyText =>
                            {
                                // Checking if this user has permission for module
                                var result = HasPermissionForModule(moduleName);

                                // If no permission for module then no permission for action too
                                if (!result)
                                    return false;

                                // Now we can check for action permissions

                                // Searching module by name
                                var module =
                                    SecuritySystem.Instance
                                    .SecuredModules
                                    .AsParallel()
                                    .FirstOrDefault(m => m.Name == moduleName);

                                // If module is null assume has permissions
                                if (module == null)
                                    return true;

                                // Searching action by name
                                var action =
                                    module.Actions
                                    .AsParallel()
                                    .FirstOrDefault(a => a.Name == actionName);

                                // Checking if action exists and this user has permission
                                result = action == null
                                      || action.CheckUserPermission(this);

                                return result;
                            }),

                    unlocked: 
                        () => _ActionPermissions)[key];
        }


        HashSet<string> _PermissionCodes;

        /// <summary>
        /// Returns all permissions for user
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GetPermissionsCodes()
        {
            return this.LockedResult(
                criteria: 
                    () => _PermissionCodes == null,
                locked: 
                    () => _PermissionCodes =
                        new HashSet<string>(
                           GetPermissions()
                           .Select(p => p.PermissionCode)),
                unlocked: 
                    () => _PermissionCodes);
        }
        

        List<Permission> _Permissions;

        /// <summary>
        /// Returns all permissions for user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermissions()
        {
            return this.LockedResult(
                criteria: 
                    () => _Permissions == null,
                locked: 
                    () => _Permissions =
                        UserGroups
                        .Where(g => g.IsEnabled)
                        .SelectMany(u =>
                            u.Roles
                            .Where(r => r.IsEnabled)
                            .SelectMany(r =>
                                r.Permissions
                                .Where(p => p.IsEnabled)))
                        .ToList(),
                unlocked: 
                    () => _Permissions);
        }


        public static bool operator ==(User user, string userName)
        {
            // Assume comparison of user's name with the string
            return user != null
                && user.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase);
        }


        public static bool operator !=(User user, string userName)
        {
            return !(user == userName);
        }
    }
}
