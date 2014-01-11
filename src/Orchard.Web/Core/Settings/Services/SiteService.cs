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
        
        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;
        private IRepository<SiteSettingsPartRecord> _siteSettingsRepository;

        public SiteService(
            IContentManager contentManager,
            ICacheManager cacheManager,
            IRepository<SiteSettingsPartRecord> siteSettingsRepository) {
            _contentManager = contentManager;
            _cacheManager = cacheManager;
            _siteSettingsRepository = siteSettingsRepository; 
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public ISite GetSiteSettings() {
            var siteId = _cacheManager.Get("SiteId", ctx => {
                var site = _siteSettingsRepository.Table.FirstOrDefault();
                if (site == null) {
                    var siteSettingsPart = DefaultSiteSettings;
                    _siteSettingsRepository.Create(siteSettingsPart.Record);
                    return siteSettingsPart.Id;
                }
                return site.Id;
            });

            return new SiteSettingsPart(_siteSettingsRepository.Get(siteId));
        }
    }
}