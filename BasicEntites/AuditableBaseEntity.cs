using System;

namespace AuditSourcesSample
{
    public abstract class AuditableBaseEntity<TKey> : BaseEntity<TKey>, IAuditUpdate, IAuditCreate
    {
        public string CreatedUserRef { get; set; }

        public string ModifiedUserRef { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }

    public abstract class AuditableBaseEntity : AuditableBaseEntity<int>
    {

    }

}
