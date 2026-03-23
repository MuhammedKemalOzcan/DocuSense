using DocuSense.Application;
using DocuSense.Infrastructure;
using DocuSense.Persistence;
using DocuSense.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DocuSense.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Yukarýdaki isimle (name: "Bearer") birebir ayný olmalý
                }
            },
            new string[] {} // Kapsamlar (scopes) boþ býrakýlýr
        }
    });
            });

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<DocuSenseAPIDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Authority = builder.Configuration["Keycloak:Authority"];
                opt.Audience = builder.Configuration["Keycloak:Audience"];
                opt.RequireHttpsMetadata = false;

                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuers = new[]
                    {
                        "http://localhost:8180/realms/docusense",
                        "http://keycloak:8080/realms/docusense"
                    },
                    ValidateAudience = true,
                    ValidAudiences = new[]
                    {
                        "docusense-client",
                        "account"
                    },

                    ValidateLifetime = true,

                    NameClaimType = "preferred_username",
                    RoleClaimType = "role",
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DocuSenseAPIDbContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}