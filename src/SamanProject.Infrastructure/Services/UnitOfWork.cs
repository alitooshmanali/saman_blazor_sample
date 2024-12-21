using Microsoft.EntityFrameworkCore;
using SamanProject.Application;
using SamanProject.Domain;
using SamanProject.Infrastructure.Context;

namespace SamanProject.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SamanDbContext dbContext;

        public UnitOfWork(SamanDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            return dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            SetAuditingProperties();

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await dbContext.Database.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private void SetAuditingProperties()
        {
            var entries = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(i => i.State == EntityState.Added || i.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var versionProperty = entry.Property("Version");
                versionProperty.CurrentValue = (int)versionProperty.OriginalValue + 1;

                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = DateTimeOffset.UtcNow;
                }

                if (entry.State != EntityState.Modified)
                    continue;

                entry.Property("Updated").CurrentValue = DateTimeOffset.UtcNow;
            }
        }
    }
}
