using Core.Repositorios;
using Infra.Database;
using Infra.Database.Repositorios;
using Infra.Servicos.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Config;
using Shared.ServiceBus;

namespace Infra.DPI;

public static class DependenciaInfra
{
    public static void AddDependenciaInfra(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConfigContantes.DefaultConnection);

        if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(ConfigContantes.DefaultConnection);

        services.AddDbContext<CreditoContext>((options) => options.UseNpgsql(connectionString));

        services.AddScoped<ICreditoRepository, CreditoRepository>();
        
        services.AddSingleton<ICreditoBusService, CreditoBusService>();
    }
}