using OrderProcessingApi.Extensions;
using OrderProcessingApi.Registrars;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices(typeof(Program));
var app = builder.Build();

app.RegisterPipelineComponents(typeof(Program));

app.Run();
