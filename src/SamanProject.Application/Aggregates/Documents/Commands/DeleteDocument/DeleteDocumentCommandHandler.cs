using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand>
    {
        private readonly IWriteDocumentRepository writeDocumentRepository;

        public DeleteDocumentCommandHandler(IWriteDocumentRepository writeDocumentRepository)
        {
            this.writeDocumentRepository = writeDocumentRepository;
        }

        public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await writeDocumentRepository.GetById(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"Unable To Found Document By {request.Id}");


            writeDocumentRepository.Remove(document);
        }
    }
}
