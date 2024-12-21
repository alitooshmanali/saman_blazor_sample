using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Queries.GetDocumentCollections
{
    public class GetDocumentCollectionQuery: BaseCollectionQuery, IRequest<BaseCollectionResult<DocumentQueryResult>>
    {
    }
}
