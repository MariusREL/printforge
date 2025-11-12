
namespace backend.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddConfiguredCors(this IServiceCollection services, IConfiguration cfg)
    {
        var section = cfg.GetSection("Cors");
        var name = section.GetValue("PolicyName", "AllowFrontend");
        var origins = section.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        var allowCreds = section.GetValue("AllowCredentials", false);

        services.AddCors(o => o.AddPolicy(name, p => {
            var qb = p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
            if (allowCreds) qb.AllowCredentials();
        }));
        return services;
    }
}