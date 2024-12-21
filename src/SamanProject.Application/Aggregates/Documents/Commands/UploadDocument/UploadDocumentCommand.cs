using MediatR;

namespace SamanProject.Application.Aggregates.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommand: IRequest<string>
    {
        public string FileName { get; set; }

        public byte[] FileContent { get; set; }

        public string FileExtension { get; set; }

        public string EntityType { get; set; }

        public Guid EntityId { get; set; }

        public string UploadedBy { get; set; }
    }
}
