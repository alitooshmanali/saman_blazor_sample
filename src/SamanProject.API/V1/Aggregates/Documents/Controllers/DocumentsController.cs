using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestAPI.V1.Models;
using SamanProject.API.V1.Aggregates.Documents.Models;
using SamanProject.Application.Aggregates.Documents.Commands.DeleteDocument;
using SamanProject.Application.Aggregates.Documents.Commands.UploadDocument;
using SamanProject.Application.Aggregates.Documents.Queries.GetDocumentById;
using SamanProject.Application.Aggregates.Documents.Queries.GetDocumentCollections;

namespace SamanProject.API.V1.Aggregates.Documents.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("rest/api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]

    public class DocumentsController: ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        public DocumentsController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] DocumentRequest request, CancellationToken cancellationToken)
        {
            var command = mapper.Map<UploadDocumentCommand>(request);
            var fileName = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            
            return Ok(fileName);
        }


        [HttpGet]
        public async Task<ActionResult<ResponseCollectionModel<DocumentResponse[]>>> GetAllDocuments(
            [FromQuery] SearchModel searchModel,
            CancellationToken cancellationToken)
        {
            var query = mapper.Map<GetDocumentCollectionQuery>(searchModel);
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            return Ok(new ResponseCollectionModel<DocumentResponse>
            {
                Values = mapper.Map<DocumentResponse[]>(queryResult.Result),
                TotalCount = queryResult.TotalCount
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseModel<DocumentResponse>>> GetDocumentById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetDocumentByIdQuery { Id = id };
            var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (queryResult is null)
                return NotFound();

            return Ok(new ResponseModel<DocumentResponse> { Values = mapper.Map<DocumentResponse>(queryResult) });
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteDocument(Guid id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteDocumentCommand { Id = id }, cancellationToken)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}
