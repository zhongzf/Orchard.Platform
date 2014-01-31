using System;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.Data.Conventions;
using Orchard.Settings;

namespace Orchard.Core.Settings.Models {
    public sealed class SiteSettingsPart : ContentPart<SiteSettingsPartRecord>, ISite {

        public SiteSettingsPart(int id, string siteSalt, string siteName, string pageTitleSeparator, string siteTimeZone)
            : this(new SiteSettingsPartRecord(id, siteSalt, siteName, pageTitleSeparator, siteTimeZone))
        {
        }

        public SiteSettingsPart(SiteSettingsPartRecord record)
            : this()
        {
            Record = record;
            ContentItem = new ContentItem
            {
                VersionRecord = new ContentManagement.Records.ContentItemVersionRecord
                {
                    Id = record.Id,
                    ContentItemRecord = new ContentManagement.Records.ContentItemRecord
                    {
                        Id = record.Id
                    }
                },
                ContentType = "Site"
            };
        }

        public SiteSettingsPart()
        {
        }

        public string PageTitleSeparator {
            get { return Record.PageTitleSeparator; }
            set { Record.PageTitleSeparator = value; }
        }

        public string SiteName {
            get { return Record.SiteName; }
            set { Record.SiteName = value; }
        }

        public string SiteSalt {
            get { return Record.SiteSalt; }
        }

        public string SuperUser {
            get { return Record.SuperUser; }
            set { Record.SuperUser = value; }
        }

        public string HomePage {
            get { return Record.HomePage; }
            set { Record.HomePage = value; }
        }

        public string SiteCulture {
            get { return Record.SiteCulture; }
            set { Record.SiteCulture = value; }
        }

        public ResourceDebugMode ResourceDebugMode {
            get { return Record.ResourceDebugMode; }
            set { Record.ResourceDebugMode = value; }
        }

        public int PageSize {
            get { return Record.PageSize; }
            set { Record.PageSize = value; }
        }

        public string SiteTimeZone {
            get { return Record.SiteTimeZone; }
            set { Record.SiteTimeZone = value; }
        }

        [StringLengthMax]
        public string BaseUrl {
            get {
                return this.As<SiteSettings2Part>().BaseUrl;
            }
            set {
                this.As<SiteSettings2Part>().BaseUrl = value;
            }
        }
    }
}
