using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Infrastructure.Notification;
using eduflowbackend.Infrastructure.Repositories;
using eduflowbackend.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace eduflowbackend.Infrastructure;

public static class ServiceRegistrationExtensions
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
        });
        
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        
        builder.Services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:8080/realms/eduflow";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = ["account"],
                    ValidIssuers = ["http://localhost:8080/realms/eduflow"],
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        // Add Keycloak config and validate
        builder.Services
            .AddOptions<KeycloakConfig>()
            .Bind(builder.Configuration.GetSection(KeycloakConfig.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddHttpClient<KeycloakService>();
        builder.Services.AddScoped<KeycloakService>();

        builder.Services.AddScoped<IIdentityProviderService, KeycloakService>();

        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services
            .AddFluentEmail(builder.Configuration["Email:SenderEmail"], builder.Configuration["Email:SenderName"])
            .AddSmtpSender(builder.Configuration["Email:Host"], builder.Configuration.GetValue<int>("Email:Port"));
        
    }
}