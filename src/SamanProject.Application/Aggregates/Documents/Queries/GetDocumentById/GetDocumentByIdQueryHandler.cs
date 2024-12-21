using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Queries.GetDocumentById
{
    public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentQueryResult>
    {
        private readonly IReadDocumentRepository readDocumentRepository;

        public GetDocumentByIdQueryHandler(IReadDocumentRepository readDocumentRepository)
        {
            this.readDocumentRepository = readDocumentRepository;
        }

        public async Task<DocumentQueryResult> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
            => await readDocumentRepository.GetById(request.Id, cancellationToken).ConfigureAwait(false);
    }
}
