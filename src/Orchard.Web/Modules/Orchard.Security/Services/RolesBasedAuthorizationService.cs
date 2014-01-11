using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Security.Services
{
    public class RolesBasedAuthorizationService : IAuthorizationService
    {
        public void CheckAccess(Orchard.Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
        }

        public bool TryCheckAccess(Orchard.Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
            return true;
        }
    }
}
