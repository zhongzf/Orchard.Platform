using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;
using Orchard.Themes.Models;

namespace Orchard.Themes.Services {
    public interface ISiteThemeService : IDependency {
        ExtensionDescriptor GetSiteTheme();
        void SetSiteTheme(string themeName);
        string GetCurrentThemeName();
    }

    public class SiteThemeService : ISiteThemeService {
        public const string CurrentThemeSignal = "SiteCurrentTheme";

        private readonly IExtensionManager _extensionManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly IOrchardServices _orchardServices;
        private readonly IRepository<ThemeSiteSettingsPartRecord> _themeSiteSettingsPartRecordRepository;

        public SiteThemeService(
            IOrchardServices orchardServices,
            IRepository<ThemeSiteSettingsPartRecord> themeSiteSettingsPartRecordRepository,
            IExtensionManager extensionManager,
            ICacheManager cacheManager,
            ISignals signals) {

            _orchardServices = orchardServices;
            _themeSiteSettingsPartRecordRepository = themeSiteSettingsPartRecordRepository;
            _extensionManager = extensionManager;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public ExtensionDescriptor GetSiteTheme() {
            string currentThemeName = GetCurrentThemeName();
            return string.IsNullOrEmpty(currentThemeName) ? null : _extensionManager.GetExtension(GetCurrentThemeName());
        }

        public void SetSiteTheme(string themeName)
        {
            var site = _orchardServices.WorkContext.CurrentSite;
            var themeSiteSettingsPart = site.As<ThemeSiteSettingsPart>();
            if (themeSiteSettingsPart != null)
            {
                themeSiteSettingsPart.CurrentThemeName = themeName;
            }
            else
            {
                var themeSiteSettingsPartRecord = _themeSiteSettingsPartRecordRepository.Get(_orchardServices.WorkContext.CurrentSite.Id);
                if (themeSiteSettingsPartRecord != null)
                {
                    themeSiteSettingsPartRecord.CurrentThemeName = themeName;
                    _themeSiteSettingsPartRecordRepository.Update(themeSiteSettingsPartRecord);
                }
                else
                {
                    themeSiteSettingsPartRecord = new ThemeSiteSettingsPartRecord
                    {
                        Id = _orchardServices.WorkContext.CurrentSite.Id,
                        CurrentThemeName = themeName
                    };
                    _themeSiteSettingsPartRecordRepository.Create(themeSiteSettingsPartRecord);
                }
            }
            _signals.Trigger(CurrentThemeSignal);
        }

        public string GetCurrentThemeName()
        {
            return _cacheManager.Get("CurrentThemeName", ctx =>
            {
                ctx.Monitor(_signals.When(CurrentThemeSignal));
                var themeSiteSettingsPart = _orchardServices.WorkContext.CurrentSite.As<ThemeSiteSettingsPart>();
                if (themeSiteSettingsPart != null)
                {
                    return themeSiteSettingsPart.CurrentThemeName;
                }
                else
                {
                    var themeSiteSettingsPartRecord = _themeSiteSettingsPartRecordRepository.Get(_orchardServices.WorkContext.CurrentSite.Id);
                    if (themeSiteSettingsPartRecord != null)
                    {
                        return themeSiteSettingsPartRecord.CurrentThemeName;
                    }
                    else
                    {
                        return "TheThemeMachine";
                    }
                }
            });
        }
    }
}
