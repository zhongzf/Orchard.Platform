using System.Web.Routing;
using JetBrains.Annotations;
using Orchard.Environment.Extensions;

namespace Orchard.Themes.Services {
    [UsedImplicitly]
    [OrchardSuppressDependency("Orchard.Themes.DefaultThemeSelector")]
    public class SafeModeThemeSelector : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context) {
            return new ThemeSelectorResult {Priority = -100, ThemeName = "Themes"};
        }
    }
}