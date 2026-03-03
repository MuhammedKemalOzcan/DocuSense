using DocuSense.Application.Services;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;
using DocuSense.Persistence.Repositories;
using DocuSense.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Connectors.PgVector;
using Npgsql;

namespace DocuSense.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DocuSenseAPIDbContext>(opt =>
            {
                opt.UseNpgsql("Host=localhost;Database=docusense;Username=postgres;Password=123");
            });

            //NpgsqlDataSource ve PostgresVectorStore nesneleri standart yöntemi kullanamayız. Çünkü bu sınıflar ayağa kalkarken bağlantı dizesine (connection string) ve .UseVector() gibi özel metot çağrılarına ihtiyaç duyar.
            NpgsqlDataSourceBuilder dataSourceBuilder = new(config.GetConnectionString("Postgres"));
            dataSourceBuilder.UseVector();
            NpgsqlDataSource dataSource = dataSourceBuilder.Build();
            var vectorStore = new PostgresVectorStore(dataSource, true);

            //konfigürasyonu dışarıda Builder nesnesi ile biz manuel olarak yaptık, nesneyi bellekte oluşturduk (Build()) ve DI konteynerine "Bunu sen üretme, bellekteki bu hazır nesneyi kullan" diyerek doğrudan (dataSource) şeklinde parametre olarak verdik.
            services.AddSingleton(dataSource);
            services.AddSingleton<Microsoft.Extensions.VectorData.VectorStore>(vectorStore);
            services.AddScoped<IVectorDatabaseService, VectorDatabaseService>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}