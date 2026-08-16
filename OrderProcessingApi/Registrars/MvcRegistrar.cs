using Asp.Versioning;
using OrderProcessingApi.Filters;

namespace OrderProcessingApi.Registrars;

public class MvcRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        var apiVersioningBuilder = builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        }); 
        
        builder.Services.AddEndpointsApiExplorer();
    }
}