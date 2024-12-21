using AutoMapper;
using DocumentManagementBlazor.V1.Aggregates.Documents;
using DocumentManagementBlazor.V1.Models;
using MediatR;
using SamanProject.Application.Aggregates.Documents.Commands.DeleteDocument;
using SamanProject.Application.Aggregates.Documents.Commands.UploadDocument;
using SamanProject.Application.Aggregates.Documents.Queries.GetDocumentById;
using SamanProject.Application.Aggregates.Documents.Queries.GetDocumentCollections;

namespace DocumentManagementBlazor.Data
{
    public class DocumentService
    {
        private readonly IMediator mediator;

        private readonly IMapper mapper;

        public DocumentService(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<string> UploadDocumentAsync(DocumentRequest request, CancellationToken cancellationToken)
        {
            var command = mapper.Map<UploadDocumentCommand>(request);
            var fileName = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return fileName;
        }

        public async Task<ResponseCollectionModel<DocumentResponse>> GetAllDocuments(SearchModel searchModel, CancellationToken cancellationToken)
        {
            var query = mapper.Map<GetDocumentCollectionQuery>(searchModel);
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            return new ResponseCollectionModel<DocumentResponse>
            {
                Values = mapper.Map<DocumentResponse[]>(queryResult.Result),
                TotalCount = queryResult.TotalCount
            };
        }

        public async Task<ResponseModel<DocumentResponse>> GetDocumentById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetDocumentByIdQuery { Id = id };

            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (queryResult is null)
                throw new FileNotFoundException($"Unable to found document by {id}");

            return new ResponseModel<DocumentResponse> { Values = mapper.Map<DocumentResponse>(queryResult) };
        }

        public async Task DeleteDocument(Guid id, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteDocumentCommand { Id = id }, cancellationToken)
                .ConfigureAwait(false);

    }
}
