using OrderProcessing.Infrastructure;

namespace OrderProcessingApi.Registrars;

public class InfrastructureRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration);
    }
}