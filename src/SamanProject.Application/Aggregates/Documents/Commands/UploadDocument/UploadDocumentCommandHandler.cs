using MediatR;
using SamanProject.Domain.Aggregates.Documents;

namespace SamanProject.Application.Aggregates.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, string>
    {
        private readonly IWriteDocumentRepository writeDocumentRepository;

        public UploadDocumentCommandHandler(IWriteDocumentRepository writeDocumentRepository)
        {
            this.writeDocumentRepository = writeDocumentRepository;
        }

        public Task<string> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = Document.Create(request.FileName,
                request.FileContent,
                request.FileExtension,
                request.EntityType,
                request.EntityId,
                request.UploadedBy);

            writeDocumentRepository.Add(document);

            return Task.FromResult(request.FileName);
        }
    }
}
