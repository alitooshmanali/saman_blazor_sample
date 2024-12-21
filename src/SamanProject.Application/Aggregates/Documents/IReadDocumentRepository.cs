using SamanProject.Application.Aggregates.Documents.Queries;

namespace SamanProject.Application.Aggregates.Documents
{
    public interface IReadDocumentRepository
    {
        IQueryable<DocumentQueryResult> GetAll();

        Task<DocumentQueryResult> GetById(Guid id, CancellationToken cancellationToken = default);

    }
}
