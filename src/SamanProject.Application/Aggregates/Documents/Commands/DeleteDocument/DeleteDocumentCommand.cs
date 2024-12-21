using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommand: IRequest
    {
        public Guid Id { get; set; }
    }
}
