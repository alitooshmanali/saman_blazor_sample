namespace SamanProject.API.V1.Aggregates.Documents.Models
{
    public class DocumentResponse
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public byte[] FileContent { get; set; }

        public string EntityType { get; set; }

        public Guid EntityId { get; set; }

        public string UploadedBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public int Version { get; set; }
    }
}
