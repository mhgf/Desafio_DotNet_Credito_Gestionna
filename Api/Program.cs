using Core.DPI;
using DesafioGestionna.Api.Handlers;
using Infra.DPI;
using Shared.Config;
using Shared.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ServiceBusConfig>(builder.Configuration.GetSection("AzureServiceBus"));

builder.Services.AddDependicaCore();
builder.Services.AddDependenciaInfra();

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