using Microsoft.EntityFrameworkCore;
using Npgsql;
using SamanProject.Application.Aggregates.Documents;
using SamanProject.Application.Aggregates.Documents.Queries;
using SamanProject.Infrastructure.Context;

namespace SamanProject.Infrastructure.Aggregates.Documents
{
    public class ReadDocumentRepository : IReadDocumentRepository
    {
        private readonly SamanDbContext _dbContext;

        public ReadDocumentRepository(SamanDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IQueryable<DocumentQueryResult> GetAll()
        => _dbContext.Database.SqlQueryRaw<DocumentQueryResult>($@"
                    SELECT      D.""Id"",
                                D.""FileName"",
                                D.""FileExtension"",
                                D.""FileContent"",
                                D.""EntityType"",
                                D.""EntityId"",
                                D.""UploadedBy"",
                                D.""Created"",
                                D.""Updated"",
                                D.""Version""
                    FROM        ""Documents"" AS D
                ");

        public async Task<DocumentQueryResult> GetById(Guid id, CancellationToken cancellationToken = default)
        =>
            await _dbContext.Database.SqlQueryRaw<DocumentQueryResult>($@"
                   SELECT       D.""Id"",
                                D.""FileName"",
                                D.""FileExtension"",
                                D.""FileContent"",
                                D.""EntityType"",
                                D.""EntityId"",
                                D.""UploadedBy"",
                                D.""Created"",
                                D.""Updated"",
                                D.""Version""
                    FROM        ""Documents"" AS D
                    WHERE       D.""Id"" = '{id}'")
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
