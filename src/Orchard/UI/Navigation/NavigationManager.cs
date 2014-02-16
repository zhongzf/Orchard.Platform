using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.UI.Navigation
{
    public class NavigationManager : INavigationManager 
    {
        public IEnumerable<MenuItem> BuildMenu(string menuName)
        {
            return new MenuItem[] { };
        }

        public IEnumerable<MenuItem> BuildMenu(ContentManagement.IContent menu)
        {
            return new MenuItem[] { };
        }

        public IEnumerable<string> BuildImageSets(string menuName)
        {
            return new string[] { };
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
