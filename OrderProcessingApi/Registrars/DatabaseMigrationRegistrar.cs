using Microsoft.EntityFrameworkCore;
using OrderProcessing.Infrastructure.Persistence;

namespace OrderProcessingApi.Registrars;

public class DatabaseMigrationRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();
        dbContext.Database.Migrate();
    }
}
