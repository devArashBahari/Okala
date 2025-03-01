using Okala.Api;
using Okala.Application;
using Okala.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog(SerilogOptions.ConfigureLogger);

builder.Services
    .AddPresentation()    //API services like cross cutting services in the API layer should be injected in this
    .AddApplication()     //Application Services and abstractions should be injected here
    .AddInfrastructure() //Infrastructure service like JWTTokenGenerator, FluentValidationConfigurations, AutoMapperConfigurations should be injected here.
    .AddProviders(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(ep => ep.MapControllers());

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quote API(v1)");
});

await app.RunAsync();