using AutoMapper;
using RestAPI.V1.Models;
using SamanProject.Application.Aggregates.Documents.Commands.UploadDocument;
using SamanProject.Application.Aggregates.Documents.Queries;
using SamanProject.Application.Aggregates.Documents.Queries.GetDocumentCollections;

namespace SamanProject.API.V1.Aggregates.Documents.Models
{
    public class DocumentMappingProfile: Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<SearchModel, GetDocumentCollectionQuery>();
            CreateMap<DocumentRequest, UploadDocumentCommand>();
            CreateMap<DocumentQueryResult, DocumentResponse>();

        }
    }
}
