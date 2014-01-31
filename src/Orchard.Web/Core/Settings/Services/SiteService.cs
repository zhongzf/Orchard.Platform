using System;
using System.Linq;
using JetBrains.Annotations;
using Orchard.Caching;
using Orchard.Core.Settings.Models;
using Orchard.Data;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.Settings;

namespace Orchard.Core.Settings.Services {
    [UsedImplicitly]
    public class SiteService : ISiteService {
        private static readonly SiteSettingsPart DefaultSiteSettings = new SiteSettingsPart(1, Guid.NewGuid().ToString("N"), "Orchard", " - ", TimeZoneInfo.Local.Id);

        private IRepository<SiteSettingsPartRecord> _siteSettingsRepository; 
        
        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;

        public SiteService(
            IRepository<SiteSettingsPartRecord> siteSettingsRepository,
            IContentManager contentManager,
            ICacheManager cacheManager) {
                _siteSettingsRepository = siteSettingsRepository;
                _contentManager = contentManager;
                _cacheManager = cacheManager;
                Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public ISite GetSiteSettings() {
            var siteId = _cacheManager.Get("SiteId", ctx => {
                var site = _contentManager.Query("Site") != null ? _contentManager.Query("Site")
                    .Slice(0, 1)
                    .FirstOrDefault() : null;

                if(site == null)
                {
                    var siteSettingPartRecord = _siteSettingsRepository.Table.FirstOrDefault();
                    if (siteSettingPartRecord == null)
                    {
                        var siteSettingsPart = DefaultSiteSettings;
                        _contentManager.Create<SiteSettingsPart>("Site", item =>
                        {
                            item.Record.SiteSalt = siteSettingsPart.SiteSalt;
                            item.Record.SiteName = siteSettingsPart.SiteName;
                            item.Record.PageTitleSeparator = siteSettingsPart.PageTitleSeparator;
                            item.Record.SiteTimeZone = siteSettingsPart.SiteTimeZone;
                        });
                        //_siteSettingsRepository.Create(siteSettingsPart.Record);
                        return siteSettingsPart.Id;
                    }
                    return siteSettingPartRecord.Id;
                }
                else
                {
                    return site.Id;
                }
            });

            return _contentManager.Get<ISite>(siteId, VersionOptions.Published, new QueryHints().ExpandRecords<SiteSettingsPartRecord>()) != null ? _contentManager.Get<ISite>(siteId, VersionOptions.Published, new QueryHints().ExpandRecords<SiteSettingsPartRecord>()) 
                : new SiteSettingsPart(_siteSettingsRepository.Get(siteId));
        }
    }
}