using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Queries.GetDocumentById
{
    public class GetDocumentByIdQuery: IRequest<DocumentQueryResult>
    {
        public Guid Id { get; set; }
    }
}
