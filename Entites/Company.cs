using System;
using System.Collections.Generic;

namespace AuditSourcesSample
{
    public class Company : FullAuditableBaseEntity
    {
        public string Name { get; set; }

        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }

    public class Employee : FullAuditableBaseEntity
    {
        public string Name { get; set; }

        public virtual List<Activity> Activities { get; set; } = new List<Activity>();
    }

    public class Activity : IAuditCreate, IAuditCreateSources
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string CreatedUserRef { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public string CreatedSources { get; set; }
    }
}
