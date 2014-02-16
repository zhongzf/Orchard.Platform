using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Security
{
    public class AuthorizationService : IAuthorizationService
    {
        public void CheckAccess(Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {            
        }

        public bool TryCheckAccess(Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
            return true;
        }
    }
}
