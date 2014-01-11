using System;
using Orchard.ContentManagement.Records;
using Orchard.Settings;

namespace Orchard.Core.Settings.Models {
    public class SiteSettingsPartRecord : ContentPartRecord {
        public const int DefaultPageSize = 10;        

        public SiteSettingsPartRecord(int id, string siteSalt, string siteName, string pageTitleSeparator, string siteTimeZone)
            : this()
        {
            Id = id;
            SiteSalt = siteSalt;
            SiteName = siteName;
            PageTitleSeparator = pageTitleSeparator;
            SiteTimeZone = siteTimeZone;
        }

        public SiteSettingsPartRecord() {
            PageSize = DefaultPageSize;
        }

        public virtual string SiteSalt { get; set; }

        public virtual string SiteName { get; set; }

        public virtual string SuperUser { get; set; }

        public virtual string PageTitleSeparator { get; set; }

        public virtual string HomePage { get; set; }

        public virtual string SiteCulture { get; set; }

        public virtual ResourceDebugMode ResourceDebugMode { get; set; }

        public virtual int PageSize { get; set; }

        public virtual string SiteTimeZone { get; set; }
    }
}