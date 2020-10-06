using System;

namespace AuditSourcesSample
{
    public abstract class FullAuditableBaseEntity<TKey> : AuditableBaseEntity<TKey>, IAuditCreateSources, IAuditCreate, IAuditUpdateSources
    {
        public string CreatedSources { get; set; }

        public string ModifiedSources { get; set; }
    }

    public abstract class FullAuditableBaseEntity : FullAuditableBaseEntity<int>
    {

    }
}