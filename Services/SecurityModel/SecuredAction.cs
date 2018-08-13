using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityModel
{
    public class SecuredAction
    {
        public SecuredAction()
        {
        }

        public SecuredModule Module { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public Permission Permission { get; set; }
    }
}
