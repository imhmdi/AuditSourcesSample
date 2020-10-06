using System;

namespace AuditSourcesSample
{
    public interface IEntity
    {

    }

    public interface IAuditCreate
    {
        string CreatedUserRef { get; set; }

        DateTimeOffset? CreatedDate { get; set; }
    }

    public interface IAuditUpdate
    {
        string ModifiedUserRef { get; set; }

        DateTimeOffset? ModifiedDate { get; set; }
    }

    public interface IAuditCreateSources
    {
        string CreatedSources { get; set; }
    }

    public interface IAuditUpdateSources
    {
        string ModifiedSources { get; set; }
    }
}