using Orchard.Data.Conventions;

namespace Orchard.Core.Settings.Metadata.Records {
    public class ContentTypePartRecord {
        public virtual int Id { get; set; }
        public virtual ContentPartRecord ContentPartRecord { get; set; }
        [StringLengthMax]
        public virtual string Settings { get; set; }
    }
}
