using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Orchard.Themes
{
    public class DefaultThemeSelector : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            return new ThemeSelectorResult { Priority = -100, ThemeName = "Themes" };
        }
    }
}
