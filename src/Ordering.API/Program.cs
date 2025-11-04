using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Messaging.RabbitMq.Extensions;
using Ordering.Persistance.Extensions;
using Ordering.Persistance.Postgres.Extensions;
using Ordering.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterApiDependencies()
    .RegisterPersistancecDependencies()
    .AddWOrker(builder.Configuration)
    .RegisterMessagingRabbitMqDependencies(builder.Configuration)
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterPostgresDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
