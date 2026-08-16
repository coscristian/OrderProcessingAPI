using OrderProcessingApi.Options;

namespace OrderProcessingApi.Registrars;

public class SwaggerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}