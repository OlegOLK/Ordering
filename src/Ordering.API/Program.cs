using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Messaging.RabbitMq.Extensions;
using Ordering.Persistance.Extensions;
using Ordering.Persistance.Postgres;
using Ordering.Persistance.Postgres.Extensions;
using Ordering.Processing.Extensions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterApiDependencies()
    .RegisterPersistancecDependencies()
    .RegisterProcessingDependencies(builder.Configuration)
    .RegisterMessagingRabbitMqDependencies(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterPostgresDependencies(builder.Configuration);

builder.Services.AddHealthChecks()
    .RegisterApiHealthCheck()
    .RegisterMessagingHealthCheck()
    .RegisterPersisntanceHealthCheck();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Ordering"))
            .AddMeter("Ordering.Processing")
            .AddPrometheusExporter();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks("/healthz/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapGet("healthz/alive", async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        await context.Response.WriteAsync(HttpStatusCode.OK.ToString()).ConfigureAwait(false);
    });

app.MapPrometheusScrapingEndpoint();
app.MapControllers();

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogWarning("Application is shuting down in 15 seconds. Finalizing working.");
    Thread.Sleep(15000); // Gracefull shutdown, can be fencier but let it be.
});

app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    OrderContext ctx = scope.ServiceProvider.GetRequiredService<OrderContext>();
    ctx.Database.EnsureCreated();
    ctx.Database.Migrate();
});

app.Run();
