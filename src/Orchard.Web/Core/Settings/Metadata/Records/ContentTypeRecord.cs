using System.Collections.Generic;
using Orchard.Data.Conventions;

namespace Orchard.Core.Settings.Metadata.Records {
    public class ContentTypeRecord  {
        public ContentTypeRecord() {
            ContentTypePartRecords = new List<ContentTypePartRecord>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool Hidden { get; set; }
        [StringLengthMax]
        public virtual string Settings { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ContentTypePartRecord> ContentTypePartRecords { get; set; }
    }

}
