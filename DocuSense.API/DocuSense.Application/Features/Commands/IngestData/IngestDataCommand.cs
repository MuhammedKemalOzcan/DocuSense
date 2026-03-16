using DocuSense.Application.Dtos;
using DocuSense.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocuSense.Application.Features.Queries.Commands.IngestData
{
    public record IngestDataCommand(IFormFile? File) : IRequest<Result<IngestResultDto>>;
}