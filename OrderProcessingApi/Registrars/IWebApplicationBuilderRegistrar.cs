namespace OrderProcessingApi.Registrars;

public interface IWebApplicationBuilderRegistrar : IRegistrar
{
    void RegisterServices(WebApplicationBuilder builder);
}