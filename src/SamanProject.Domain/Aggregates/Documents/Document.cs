namespace SamanProject.Domain.Aggregates.Documents
{
    public class Document: Entity 
    {
        private Document() { }

        public Guid Id { get; private set; }

        public string FileName { get; private set; }

        public string FileExtension { get; set; }

        public byte[] FileContent { get; private set; }

        public string EntityType { get; private set; }

        public Guid EntityId { get; private set; }

        public string UploadedBy { get; private set; }

        public static Document Create(
            string fileName,
            byte[] fileContent,
            string fileExtension,
            string entityType,
            Guid entityId,
            string uploadedBy)
        {
            var document = new Document()
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                FileContent = fileContent,
                FileExtension = fileExtension,
                EntityType = entityType,
                EntityId = entityId,
                UploadedBy = uploadedBy
            };

            // TODO: Can be added DomainEntity for traking documentAddedEvent in domain
            // ....

            return document;
        }

        public void ChangeFileName(string fileName)
        {
            // TODO: Can be added DomainEntity for traking FileNameChangedEvent in domain
            // ....

            FileName = fileName;
        }

        public void ChangeEntityType(string entityType)
        {
            // TODO: Can be added DomainEntity for traking EntityTypeChangedEvent in domain
            // ....

            EntityType = entityType;
        }

        public void ChangeEntityId(Guid entityId)
        {
            // TODO: Can be added DomainEntity for traking EntityTypeChangedEvent in domain
            // ....

            EntityId = entityId;
        }

        public void ChangeEntityId(string uploadedBy)
        {
            // TODO: Can be added DomainEntity for traking EntityTypeChangedEvent in domain
            // ....

            UploadedBy = uploadedBy;
        }

        public void Delete()
        {
            if (CanBeDeleted())
                throw new InvalidOperationException();

            MarkAsDeleted();
        }

    }
}
