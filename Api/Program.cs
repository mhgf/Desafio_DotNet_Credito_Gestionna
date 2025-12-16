using System.Text.Json.Serialization;
using Core.DPI;
using DesafioGestionna.Api.Handlers;
using Infra.DPI;
using Shared.Config;
using Shared.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new BoolJsonConverter());
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ServiceBusConfig>(builder.Configuration.GetSection("AzureServiceBus"));

builder.Services.AddDependicaCore();
builder.Services.AddDependenciaInfra(builder.Configuration);

builder.Services.AddHostedService<InserirCreditoContituidoHandler>();

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