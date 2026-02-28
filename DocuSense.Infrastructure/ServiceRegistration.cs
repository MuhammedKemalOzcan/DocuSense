using DocuSense.Application.Services;
using DocuSense.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;

#pragma warning disable SKEXP0010

namespace DocuSense.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IPdfReaderService, PdfReaderService>();
            services.AddScoped<ITextChunkerService, TextChunkerService>();
            services.AddScoped<IChatGenerationService, ChatGenerationService>();

            services.AddSingleton<InMemoryVectorStore>();
            services.AddOpenAIEmbeddingGenerator(config["OpenApi:EmbeddingModel"], config["OpenApi:ApiKey"]);
            services.AddOpenAIChatCompletion(config["OpenApi:ModelId"], config["OpenApi:ApiKey"]);
        }
    }
}