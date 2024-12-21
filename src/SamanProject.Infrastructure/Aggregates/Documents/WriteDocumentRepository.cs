using Microsoft.EntityFrameworkCore;
using SamanProject.Application.Aggregates.Documents;
using SamanProject.Domain.Aggregates.Documents;
using SamanProject.Infrastructure.Context;

namespace SamanProject.Infrastructure.Aggregates.Documents
{
    public class WriteDocumentRepository : IWriteDocumentRepository
    {
        private SamanDbContext _dbContext;

        public WriteDocumentRepository(SamanDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add(Document document)
        {
            _dbContext.Documents.Add(document);
        }

        public Task<Document> GetById(Guid id, CancellationToken cancellationToken = default)
        => _dbContext.Documents.FromSqlRaw(@"
                    SELECT      D.*
                    FROM        ""Documents"" D
                    WHERE       D.""Id"" = '{0}'
                ", id)
                .SingleOrDefaultAsync(cancellationToken);


        public void Remove(Document document)
        {
            document.Delete();
            _dbContext.Documents.Remove(document);
        }
    }
}
