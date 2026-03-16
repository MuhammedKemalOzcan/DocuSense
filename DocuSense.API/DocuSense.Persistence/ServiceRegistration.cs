using DocuSense.Application.Data;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;
using DocuSense.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocuSense.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DocuSenseAPIDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("PostgresDb"));
            });

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDocuSenseAPIDbContext>(provider => provider.GetRequiredService<DocuSenseAPIDbContext>());
        }
    }
}