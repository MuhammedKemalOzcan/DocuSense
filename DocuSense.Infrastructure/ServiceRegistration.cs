using DocuSense.Application.Services;
using DocuSense.Infrastructure.Options;
using DocuSense.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.PgVector;
using Npgsql;

#pragma warning disable SKEXP0010

namespace DocuSense.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            //NpgsqlDataSource ve PostgresVectorStore nesneleri standart yöntemi kullanamayız.Çünkü bu sınıflar ayağa kalkarken bağlantı dizesine(connection string) ve.UseVector() gibi özel metot çağrılarına ihtiyaç duyar.
            NpgsqlDataSourceBuilder dataSourceBuilder = new(config.GetConnectionString("Postgres"));
            dataSourceBuilder.UseVector();
            NpgsqlDataSource dataSource = dataSourceBuilder.Build();
            var vectorStore = new PostgresVectorStore(dataSource, true);

            //konfigürasyonu dışarıda Builder nesnesi ile biz manuel olarak yaptık, nesneyi bellekte oluşturduk(Build()) ve DI konteynerine "Bunu sen üretme, bellekteki bu hazır nesneyi kullan" diyerek doğrudan(dataSource) şeklinde parametre olarak verdik.
            services.AddSingleton(dataSource);
            services.AddSingleton<Microsoft.Extensions.VectorData.VectorStore>(vectorStore);

            services.AddHttpClient<IKeycloakService, KeycloakService>();

            services.AddScoped<IPdfReaderService, PdfReaderService>();
            services.AddScoped<ITextChunkerService, TextChunkerService>();
            services.AddScoped<IChatGenerationService, ChatGenerationService>();
            services.AddScoped<IVectorDatabaseService, VectorDatabaseService>();

            services.AddOpenAIEmbeddingGenerator(config["OpenApi:EmbeddingModel"], config["OpenApi:ApiKey"]);
            services.AddOpenAIChatCompletion(config["OpenApi:ModelId"], config["OpenApi:ApiKey"]);

            services.Configure<JwtSettings>(options => config.GetSection("JwtSettings").Bind(options));
        }
    }
}