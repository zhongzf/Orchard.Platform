using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Orchard.Users.Handlers
{
    public class AuthorizationServiceEventHandler : IAuthorizationServiceEventHandler
    {
        private readonly IOrchardServices _orchardServices;

        public AuthorizationServiceEventHandler(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        private bool MembershipInitialized
        {
            get
            {
                return _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().List().Count() > 0;
            }
        }

        public void Checking(CheckAccessContext context)
        {
        }

        public void Adjust(CheckAccessContext context)
        {
            if (context.User == null && !MembershipInitialized)
            {
                context.Granted = true;
            }
        }

        public void Complete(CheckAccessContext context)
        {
        }
    }
}