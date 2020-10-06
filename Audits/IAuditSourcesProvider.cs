using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditSourcesSample
{
    public interface IAuditSourcesProvider
    {
        AuditSourceValues GetAuditSourceValues();
    }
}
