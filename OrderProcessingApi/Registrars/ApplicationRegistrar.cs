using OrderProcessing.Application;

namespace OrderProcessingApi.Registrars;

public class ApplicationRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
    }
}