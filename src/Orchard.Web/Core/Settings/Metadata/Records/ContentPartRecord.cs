using System.Collections.Generic;
using Orchard.Data.Conventions;

namespace Orchard.Core.Settings.Metadata.Records {
    public class ContentPartRecord {
        public ContentPartRecord() {
            ContentPartFieldRecords = new List<ContentPartFieldRecord>();
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Hidden { get; set; }
        [StringLengthMax]
        public virtual string Settings { get; set; }

        [CascadeAllDeleteOrphan]
        public virtual IList<ContentPartFieldRecord> ContentPartFieldRecords { get; set; }

    }
}
