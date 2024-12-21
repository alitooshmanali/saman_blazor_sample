using SamanProject.Domain.Aggregates.Documents;

namespace SamanProject.Application.Aggregates.Documents
{
    public interface IWriteDocumentRepository
    {
        void Add(Document document);

        Task<Document> GetById(Guid id, CancellationToken cancellationToken = default);

        void Remove(Document document);

    }
}
