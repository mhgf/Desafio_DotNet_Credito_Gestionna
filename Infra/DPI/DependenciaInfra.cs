using Core.Servicos.ServiceBus;
using Infra.Servicos.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DPI;

public static class DependenciaInfra
{
    public static void AddDependenciaInfra(this IServiceCollection services)
    {
        services.AddSingleton<ICreditoBusService, CreditoBusService>();
    }
}