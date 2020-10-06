using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

namespace AuditSourcesSample
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
            //var userId =_httpContextAccessor.HttpContext?.User?.Identity?.GetUserId();
            
            //for this sample project
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var now = DateTimeOffset.UtcNow;

            ApplayCreateAudits(changeTracker, currentUserId, now);
            ApplayUpdateAudits(changeTracker, currentUserId, now);
        }

        private void ApplayCreateAudits(ChangeTracker changeTracker, string currentUserId, DateTimeOffset now)
        { 
            var addedEntries = changeTracker.Entries<IAuditCreate>()
                                            .Where(x => x.State == EntityState.Added);
             
            foreach (var addedEntry in addedEntries)
            {
                addedEntry.Entity.CreatedDate = now;
                addedEntry.Entity.CreatedUserRef = currentUserId;
            }
        }

        private void ApplayUpdateAudits(ChangeTracker changeTracker, string currentUserId, DateTimeOffset now)
        {
            var addedEntries = changeTracker.Entries<IAuditUpdate>()
                                            .Where(x => x.State == EntityState.Modified);

            foreach (var addedEntry in addedEntries)
            {
                addedEntry.Entity.ModifiedDate = now;
                addedEntry.Entity.ModifiedUserRef = currentUserId;
            }
        }

    }
}

