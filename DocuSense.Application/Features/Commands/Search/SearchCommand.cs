using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Queries.Commands.Search
{
    public record SearchCommand(string Query, int Top, string ChatId) : IStreamRequest<Result<string>>;
}