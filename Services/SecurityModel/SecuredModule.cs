﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public class SecuredModule
    {
        public SecuredModule()
        {
            Actions = new List<SecuredAction>();
        }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public Permission Permission { get; set; }

        public bool IsEnabled { get; set; }
                
        public ICollection<SecuredAction> Actions { get; }


        // Checks user permission for module execution
        public bool CheckUserPermission(User user)
        {
            return IsEnabled
                 && (Permission == null
                  || user.HasPermission(Permission.PermissionCode));
        }
    }
}
