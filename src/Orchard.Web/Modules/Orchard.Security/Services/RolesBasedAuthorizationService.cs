using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Security.Services
{
    public class RolesBasedAuthorizationService : IAuthorizationService
    {
        public RolesBasedAuthorizationService()
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        public void CheckAccess(Orchard.Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
            if (!TryCheckAccess(permission, user, content))
            {
                throw new OrchardSecurityException(T("A security exception occurred in the content management system."))
                {
                    PermissionName = permission.Name,
                    User = user,
                    Content = content
                };
            }
        }

        public bool TryCheckAccess(Orchard.Security.Permissions.Permission permission, IUser user, ContentManagement.IContent content)
        {
            return true;
        }
    }
}
