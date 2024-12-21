using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SamanProject.Domain.Aggregates.Documents;
using System.Reflection;

namespace SamanProject.Infrastructure.Context
{
    public class SamanDbContext: DbContext
    {
        public SamanDbContext(DbContextOptions<SamanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("citext");
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }

#if DEBUG
    public class DesignTimeResourceDbContextFactory : IDesignTimeDbContextFactory<SamanDbContext>
    {
        public SamanDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SamanDbContext>();

            builder.UseNpgsql("Username=U;Password=P;Database=D;Host=localhost");

            return new(builder.Options);
        }
    }
#endif

}
