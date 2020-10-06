using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace AuditSourcesSample
{
    public class AuditSourcesSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IAuditSourcesProvider _auditSourcesProvider;

        public AuditSourcesSaveChangesInterceptor(IAuditSourcesProvider auditSourcesProvider)
        {
            _auditSourcesProvider = auditSourcesProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplayAudits(eventData.Context.ChangeTracker);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplayAudits(eventData.Context.ChangeTracker);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplayAudits(ChangeTracker changeTracker)
        {
            var auditSourcevalues = _auditSourcesProvider.GetAuditSourceValues().SerializeJson();

            ApplayCreateAudits(changeTracker, auditSourcevalues);
            ApplayUpdateAudits(changeTracker, auditSourcevalues);
        }

        private void ApplayCreateAudits(ChangeTracker changeTracker, string auditSourcevalues)
        { 
            var addedEntries = changeTracker.Entries<IAuditCreateSources>()
                                            .Where(x => x.State == EntityState.Added);
             
            foreach (var addedEntry in addedEntries)
            {
                addedEntry.Entity.CreatedSources = auditSourcevalues;
            }
        }

        private void ApplayUpdateAudits(ChangeTracker changeTracker, string auditSourcevalues)
        {
            var modifiedEntries = changeTracker.Entries<IAuditUpdateSources>()
                                .Where(x => x.State == EntityState.Modified);

            foreach (var modifiedEntry in modifiedEntries)
            {
                modifiedEntry.Entity.ModifiedSources = auditSourcevalues;
            }
        }
    }
}

