using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public class PermissionUsage
    {
        public PermissionUsage()
        {
        }

        public Permission Permission { get; set; }

        public Role Role { get; set; }

        public UserGroup UserGroup { get; set; }

        public User User { get; set; }
    }
}
