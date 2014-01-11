using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Security.Permissions;
using Orchard.UI;
using Orchard.UI.Navigation;
using Orchard.Utility;

namespace Orchard.Core.Navigation.Services {
    public class NavigationManager : INavigationManager {

        public NavigationManager()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public IEnumerable<MenuItem> BuildMenu(string menuName)
        {
            return new List<MenuItem>();
        }

        public IEnumerable<MenuItem> BuildMenu(ContentManagement.IContent menu)
        {
            return new List<MenuItem>();
        }

        public IEnumerable<string> BuildImageSets(string menuName)
        {
            return new List<string>();
        }

        public string GetUrl(string menuItemUrl, System.Web.Routing.RouteValueDictionary routeValueDictionary)
        {
            return string.Empty;
        }

        public string GetNextPosition(ContentManagement.IContent menu)
        {
            return string.Empty;
        }
    }
}