namespace DocumentManagementBlazor.V1.Aggregates.Documents
{
    public class DocumentRequest
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public byte[] FileContent { get; set; }

        public string EntityType { get; set; }

        public Guid EntityId { get; set; }

        public string UploadedBy { get; set; }
    }
}
