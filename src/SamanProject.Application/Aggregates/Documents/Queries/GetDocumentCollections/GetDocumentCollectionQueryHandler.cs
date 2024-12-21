using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Queries.GetDocumentCollections
{
    public class GetDocumentCollectionQueryHandler : IRequestHandler<GetDocumentCollectionQuery, BaseCollectionResult<DocumentQueryResult>>
    {
        private readonly IReadDocumentRepository readDocumentRepository;

        public GetDocumentCollectionQueryHandler(IReadDocumentRepository readDocumentRepository)
        {
            this.readDocumentRepository = readDocumentRepository;
        }

        public Task<BaseCollectionResult<DocumentQueryResult>> Handle(GetDocumentCollectionQuery request, CancellationToken cancellationToken)
        {
            var source = readDocumentRepository.GetAll().OrderByDescending(i => i.Created);
            var results = source
                .ApplyPagination(request.PageIndex, request.PageSize)
                .ToArray();

            return Task.FromResult(new BaseCollectionResult<DocumentQueryResult>
            {
                Result = results,
                TotalCount = results.Count()
            });
        }
    }
}
